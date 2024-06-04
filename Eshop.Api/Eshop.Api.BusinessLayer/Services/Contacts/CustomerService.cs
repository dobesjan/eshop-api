﻿using Eshop.Api.DataAccess.UnitOfWork;
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

        public bool CreateCustomer(Customer customer)
        {
            if (customer.IsLogged && String.IsNullOrEmpty(customer.UserId)) throw new InvalidDataException("Unknown user identity");
            if (!customer.IsLogged && String.IsNullOrEmpty(customer.Token)) throw new ArgumentNullException("Token is missing for unauthenticated user");

            if (customer.Person != null)
            {
                var person = _unitOfWork.PersonRepository.Add(customer.Person, true);
                customer.PersonId = person.Id;
            }

            if (customer.Address != null)
            {
                var address = _unitOfWork.AddressRepository.Add(customer.Address, true);
                customer.AddressId = address.Id;
            }

            _unitOfWork.CustomerRepository.Add(customer);
            _unitOfWork.CustomerRepository.Save();

            return true;
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
