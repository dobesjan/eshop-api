using Eshop.Api.DataAccess.UnitOfWork;
using Eshop.Api.Models.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.BusinessLayer.Services.Contacts
{
    public class CustomerService : EshopService, ICustomerService
    {
        private IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Customer CreateCustomer(Customer customer)
        {
            if (customer.IsLogged && String.IsNullOrEmpty(customer.UserId)) throw new InvalidDataException("Unknown user identity");
            if (!customer.IsLogged && String.IsNullOrEmpty(customer.Token)) throw new ArgumentNullException("Token is missing for unauthenticated user");
            if (customer.Contact == null) throw new InvalidDataException("Invalid contact information");

            if (customer.Contact.Person != null)
            {
                var person = _unitOfWork.PersonRepository.Add(customer.Contact.Person, true);
                customer.Contact.PersonId = person.Id;
            }

            if (customer.Contact.Address != null)
            {
                var address = _unitOfWork.AddressRepository.Add(customer.Contact.Address, true);
                customer.Contact.AddressId = address.Id;
            }

            var contact = _unitOfWork.ContactRepository.Add(customer.Contact, true);
            if (contact == null) throw new ArgumentNullException("Error storing contact");
            customer.ContactId = contact.Id;

            return _unitOfWork.CustomerRepository.Add(customer, true);
        }

        public Customer GetCustomer(int customerId)
        {
            return _unitOfWork.CustomerRepository.GetCustomer(customerId);
        }

        public Customer GetCustomerByToken(string token)
        {
            return _unitOfWork.CustomerRepository.GetCustomerByToken(token);
        }

        public Customer GetCustomerByUserId(string userId)
        {
            return _unitOfWork.CustomerRepository.GetCustomerByUserId(userId);
        }
    }
}
