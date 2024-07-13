using Eshop.Api.BusinessLayer.Services.Contacts;
using Eshop.Api.BusinessLayer.Services.Currencies;
using Eshop.Api.BusinessLayer.Services.Orders;
using Eshop.Api.DataAccess.Repository.Contacts;
using Eshop.Api.DataAccess.Repository.Orders;
using Eshop.Api.DataAccess.UnitOfWork;
using Eshop.Api.Models.Contacts;
using Eshop.Api.Models.Orders;
using Eshop.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eshop.UI.Controllers
{
    public class OrderController : EshopBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public ContactVM AddressVM { get; set; }

		[BindProperty]
        public ShippingVM ShippingVM { get; set; }

		[BindProperty]
		public PaymentMethodVM PaymentMethodVM { get; set; }

		public OrderController(
			ICustomerService customerService,
			ILogger<AccountController> logger,
			IOrderService orderService,
			IUnitOfWork unitOfWork
			) : base(customerService, logger)
        {
            _orderService = orderService;
            _unitOfWork = unitOfWork;

            if (AddressVM != null)
            {
                InitializeCountries();
            }
        }

        public IActionResult Index()
        {
            var customer = GetCustomer();
            var cart = _orderService.GetShoppingCart(customer.Id);
            return View(cart);
        }

        public IActionResult Address()
        {
			if (AddressVM == null)
			{
				AddressVM = new ContactVM();
			}

            AddressVM.BillingContact = new Contact();
            AddressVM.DeliveryAddress = new Address();

            var customer = GetCustomer();
            var cart = _orderService.GetShoppingCart(customer.Id);
            InitializeCountries();

            if (cart.BillingContact == null)
            {
                if (customer.Contact != null)
                {
                    var person = customer.Contact.Person;

                    if (person != null)
                    {
                        AddressVM.BillingContact.PersonId = person.Id;
                        AddressVM.BillingContact.Person = person;

                        if (customer.Contact.AddressId.HasValue)
                        {
                            var address = customer.Contact.Address;
                            if (address != null)
                            {
                                AddressVM.BillingContact.AddressId = address.Id;
                                AddressVM.BillingContact.Address = address;
                            }
                        }
                    }
                }
            }
            else
            {
                AddressVM.BillingContact = cart.BillingContact;
            }

			if (cart.DeliveryAddress != null)
			{
				AddressVM.DeliveryAddress = cart.DeliveryAddress;
			}

            return View(AddressVM);
        }

        [HttpPost]
        public IActionResult Address(ContactVM vm)
        {
            return HandleResponse(() =>
            {
                if (vm.BillingContact == null) return View(vm);
                InitializeCountries();

                var customer = GetCustomer();
                vm.BillingContact.Address.CustomerId = customer.Id;
                vm.DeliveryAddress.CustomerId = customer.Id;
                _orderService.LinkBillingContactToOrder(vm.BillingContact, customer.Id);
                _orderService.LinkDeliveryAddressToOrder(vm.DeliveryAddress);

                return RedirectToAction("Shipping");
            }, View(vm));
        }

		public IActionResult Shipping()
		{
			if (ShippingVM == null)
			{
                ShippingVM = new ShippingVM();
            }
			
            InitializeShippingOptions();

			var customer = GetCustomer();
			var cart = _orderService.GetShoppingCart(customer.Id);
			if (cart.ShippingId.HasValue)
            {
                ShippingVM.ShippingId = cart.ShippingId.Value;
            }

			return View(ShippingVM);
		}

		[HttpPost]
		public IActionResult Shipping(ShippingVM vm)
		{
            return HandleResponse(() =>
            {
                if (vm == null) return View(vm);

                var customer = GetCustomer();
                _orderService.UpdateShipping(vm.ShippingId, customer.Id);

                return RedirectToAction("Payment");
            }, View(vm));
        }

		public IActionResult Payment()
		{
			if (PaymentMethodVM == null)
			{
                PaymentMethodVM = new PaymentMethodVM();
            }

			var customer = GetCustomer();
			var cart = _orderService.GetShoppingCart(customer.Id);
			if (!cart.ShippingId.HasValue) return RedirectToAction("Shipping");

			InitializePaymentOptions(cart.ShippingId.Value);

			//TODO: Consider if is worth resolve payment relation like this
			if (cart.Payment != null) PaymentMethodVM.PaymentMethodId = cart.Payment.PaymentMethod.Id;

			return View(PaymentMethodVM);
		}

		[HttpPost]
		public IActionResult Payment(PaymentMethodVM vm)
		{
            return HandleResponse(() =>
            {
                if (vm == null) return View(vm);

                var customer = GetCustomer();
                var cart = _orderService.GetShoppingCart(customer.Id);
                _orderService.GeneratePayment(cart.Id, vm.PaymentMethodId);

                //TODO: COnsider if it's worth here
                cart.IsReadyToSend();

                //TODO: Switch actions based on payment type
                return RedirectToAction("Recapitulation");
            }, View(vm));
        }

		public IActionResult Recapitulation()
		{
			var customer = GetCustomer();
			var order = _orderService.GetShoppingCart(customer.Id);

			return View(order);
		}

		[HttpPost]
		public IActionResult Recapitulation(Order order)
		{
            return HandleResponse(() =>
            {
                if (order == null) return RedirectToAction("Index");

                var customer = GetCustomer();
                _orderService.SendOrder(customer.Id);

                return RedirectToAction("Sent", new { order = order });
            }, Redirect("/"));
        }

		public IActionResult Sent()
		{
            var customer = GetCustomer();
            var cart = _orderService.GetShoppingCart(customer.Id);
			if (cart == null) return RedirectToAction("Index");

            _orderService.SendOrder(customer.Id);

            return View(cart);
        }

		#region SelectListInit
		private void InitializeCountries()
        {
            IEnumerable<Country> countries = _unitOfWork.CountryRepository.GetCountries();

            if (countries != null && countries.Any())
            {
                AddressVM.Countries = countries.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
        }

		private void InitializeShippingOptions()
		{
			IEnumerable<Shipping> shippingOptions = _unitOfWork.ShippingRepository.GetEnabledShippingOptions();

			if (shippingOptions != null && shippingOptions.Any())
			{
				ShippingVM.ShippingOptions = shippingOptions.Select(i => new SelectListItem
				{
					Text = i.Name,
					Value = i.Id.ToString()
				});
			}
		}

		private void InitializePaymentOptions(int shippingId)
		{
			IEnumerable<PaymentMethod> paymentMethods = _orderService.GetPaymentMethodsForShipping(shippingId);

			if (paymentMethods != null && paymentMethods.Any())
			{
				PaymentMethodVM.PaymentMethods = paymentMethods.Select(i => new SelectListItem
				{
					Text = i.Name,
					Value = i.Id.ToString()
				});
			}
		}
		#endregion
	}
}
