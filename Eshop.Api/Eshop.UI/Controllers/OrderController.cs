using Eshop.Api.BusinessLayer.Services.Contacts;
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

		public OrderController(ICustomerService customerService, ILogger<AccountController> logger, IOrderService orderService, IUnitOfWork unitOfWork) : base(customerService, logger)
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
            ContactVM addressVM = new ContactVM();
            addressVM.BillingContact = new Contact();
            addressVM.DeliveryAddress = new Address();

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
                        addressVM.BillingContact.PersonId = person.Id;
                        addressVM.BillingContact.Person = person;

                        if (customer.Contact.AddressId.HasValue)
                        {
                            var address = _unitOfWork.AddressRepository.Get(customer.Contact.AddressId.Value);
                            if (address != null)
                            {
                                addressVM.BillingContact.AddressId = address.Id;
                                addressVM.BillingContact.Address = address;
                            }
                        }
                    }
                }
                else
                {
                    addressVM.BillingContact = cart.BillingContact;
                }

                return View(addressVM);
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
                vm.BillingContact.CustomerId = customer.Id;
                vm.BillingContact.Address.CustomerId = customer.Id;
                vm.DeliveryAddress.CustomerId = customer.Id;
                _orderService.LinkBillingContactToOrder(vm.BillingContact);
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
			ShippingVM shippingVM = new ShippingVM();
            InitializeShippingOptions();

			try
			{
				var customer = GetCustomer();
				var cart = _orderService.GetShoppingCart(customer.Id);
				if (cart.Shipping != null)
                {
                    shippingVM.Shipping = cart.Shipping;
                }

				return View(shippingVM);
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
			if (vm.Shipping == null) return View(vm);

			try
			{
				var customer = GetCustomer();
				_orderService.UpdateShipping(vm.Shipping.Id, customer.Id);

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
				AddressVM.Countries = shippingOptions.Select(i => new SelectListItem
				{
					Text = i.Name,
					Value = i.Id.ToString()
				});
			}
		}
	}
}
