using Eshop.Api.BusinessLayer.Services.Contacts;
using Eshop.Api.BusinessLayer.Services.Currencies;
using Eshop.Api.BusinessLayer.Services.Orders;
using Eshop.Api.DataAccess.Repository.Contacts;
using Eshop.Api.DataAccess.Repository.Orders;
using Eshop.Api.DataAccess.UnitOfWork;
using Eshop.Api.Models.Contacts;
using Eshop.Api.Models.Orders;
using Eshop.Api.Models.ViewModels.Orders;
using Eshop.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eshop.UI.Controllers
{
    public class OrderController : EshopBaseController
    {
		private readonly ICurrencyService _currencyService;
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
			IUnitOfWork unitOfWork,
			ICurrencyService currencyService
			) : base(customerService, logger)
        {
			_currencyService = currencyService;
            _orderService = orderService;
            _unitOfWork = unitOfWork;

            if (AddressVM != null)
            {
                InitializeCountries();
            }
        }

        public IActionResult Index()
        {
            try
            {
                var customer = GetCustomer();
                var cart = _orderService.GetShoppingCart(customer.Id);
                return View(cart);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return Redirect("/");
        }

        public IActionResult Address()
        {
			if (AddressVM == null)
			{
				AddressVM = new ContactVM();
			}

            AddressVM.BillingContact = new Contact();
            AddressVM.DeliveryAddress = new Address();

            try
            {
                var customer = GetCustomer();
                var cart = _orderService.GetShoppingCart(customer.Id);
                InitializeCountries();

                if (cart.BillingContact == null)
                {
                    var person = _unitOfWork.PersonRepository.Get(customer.Contact.PersonId);

                    if (person != null)
                    {
                        AddressVM.BillingContact.PersonId = person.Id;
                        AddressVM.BillingContact.Person = person;

                        if (customer.Contact.AddressId.HasValue)
                        {
                            var address = _unitOfWork.AddressRepository.Get(customer.Contact.AddressId.Value);
                            if (address != null)
                            {
                                AddressVM.BillingContact.AddressId = address.Id;
                                AddressVM.BillingContact.Address = address;
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
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return Redirect("/");
        }

        [HttpPost]
        public IActionResult Address(ContactVM vm)
        {
            if (vm.BillingContact == null) return View(vm);

            try
            {
                var customer = GetCustomer();
                vm.BillingContact.Address.CustomerId = customer.Id;
                vm.DeliveryAddress.CustomerId = customer.Id;
                _orderService.LinkBillingContactToOrder(vm.BillingContact, customer.Id);
                //TODO: Problem due to two address methods..
                _orderService.LinkDeliveryAddressToOrder(vm.DeliveryAddress);

                return RedirectToAction("Shipping");
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex.Message);
            }
            catch (InvalidDataException ex)
            {
                //TODO: Consider how to handle errors
                _logger.LogInformation(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return View(vm);
        }

		public IActionResult Shipping()
		{
			if (ShippingVM == null)
			{
                ShippingVM = new ShippingVM();
            }
			
            InitializeShippingOptions();

			try
			{
				var customer = GetCustomer();
				var cart = _orderService.GetShoppingCart(customer.Id);
				if (cart.ShippingId.HasValue)
                {
                    ShippingVM.ShippingId = cart.ShippingId.Value;
                }

				return View(ShippingVM);
			}
			catch (UnauthorizedAccessException ex)
			{
				_logger.LogError(ex.Message);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return Redirect("/");
		}

		[HttpPost]
		public IActionResult Shipping(ShippingVM vm)
		{
			if (vm == null) return View(vm);

			try
			{
				var customer = GetCustomer();
				_orderService.UpdateShipping(vm.ShippingId, customer.Id);

				return RedirectToAction("Payment");
			}
			catch (UnauthorizedAccessException ex)
			{
				_logger.LogError(ex.Message);
			}
			catch (InvalidDataException ex)
			{
				//TODO: Consider how to handle errors
				_logger.LogInformation(ex.Message);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return View(vm);
		}

		public IActionResult Payment()
		{
			if (PaymentMethodVM == null)
			{
                PaymentMethodVM = new PaymentMethodVM();
            }

			try
			{
				var customer = GetCustomer();
				var cart = _orderService.GetShoppingCart(customer.Id);
				if (!cart.ShippingId.HasValue) return RedirectToAction("Shipping");

				InitializePaymentOptions(cart.ShippingId.Value);

				//TODO: Consider if is worth resolve payment relation like this
				if (cart.Payment != null) PaymentMethodVM.PaymentMethodId = cart.Payment.PaymentMethod.Id;

				return View(PaymentMethodVM);
			}
			catch (UnauthorizedAccessException ex)
			{
				_logger.LogError(ex.Message);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return Redirect("/");
		}

		[HttpPost]
		public IActionResult Payment(PaymentMethodVM vm)
		{
			if (vm == null) return View(vm);

			try
			{
				var customer = GetCustomer();
				var cart = _orderService.GetShoppingCart(customer.Id);
				_orderService.GeneratePayment(cart.Id, vm.PaymentMethodId);

				cart.IsReadyToSend();

				//TODO: Switch actions based on payment type
				return RedirectToAction("Recapitulation");
			}
			catch (UnauthorizedAccessException ex)
			{
				_logger.LogError(ex.Message);
			}
			catch (InvalidDataException ex)
			{
				//TODO: Consider how to handle errors
				_logger.LogInformation(ex.Message);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return View(vm);
		}

		public IActionResult Recapitulation()
		{
			try
			{
				var customer = GetCustomer();
				var order = _orderService.GetShoppingCart(customer.Id);

				return View(order);
			}
			catch (UnauthorizedAccessException ex)
			{
				_logger.LogError(ex.Message);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return Redirect("/");
		}

		[HttpPost]
		public IActionResult Recapitulation(Order order)
		{
			if (order == null) return RedirectToAction("Index");

			try
			{
				var customer = GetCustomer();

				_orderService.SendOrder(customer.Id);

				return RedirectToAction("Sent", new { order = order });
			}
			catch (UnauthorizedAccessException ex)
			{
				_logger.LogError(ex.Message);
			}
			catch (InvalidDataException ex)
			{
				//TODO: Consider how to handle errors
				_logger.LogInformation(ex.Message);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return Redirect("/");
		}

		public IActionResult Sent(Order order)
		{
			if (order == null) return RedirectToAction("Index");
			return View(order);
		}

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
	}
}
