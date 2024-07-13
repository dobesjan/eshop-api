using Eshop.Api.Models.Contacts;
using Eshop.Api.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.BusinessLayer.Services.Orders
{
	public partial class OrderService : EshopService, IOrderService
	{
		#region Contacts
		public bool LinkDeliveryAddressToOrder(Address address)
		{
			Order order = null;

			if (address == null) throw new ArgumentNullException("Address not provided");
			if (address.CustomerId <= 0) throw new ArgumentNullException("Customer identity unknown!");

			order = GetShoppingCart(address.CustomerId);

			if (order == null)
			{
				throw new InvalidDataException("Order not found in db!");
			}

			order.DeliveryAddress = address;

			if (order.AddressId.HasValue)
			{
				order.DeliveryAddress.Id = order.AddressId.Value;
			}

			var selectedAddress = UpsertEntity(address, _unitOfWork.AddressRepository);

			if (selectedAddress != null)
			{
				order.AddressId = selectedAddress.Id;

				_unitOfWork.OrderRepository.Update(order);
				_unitOfWork.OrderRepository.Save();

				return true;
			}

			throw new InvalidDataException("Error!");
		}

		public bool LinkBillingContactToOrder(Contact contact, int customerId)
		{
			if (contact == null) throw new ArgumentNullException("Customer is null");
			if (contact.Person == null) throw new ArgumentNullException("Person is null");
			if (contact.Address == null) throw new ArgumentNullException("Address is null");

			Order order = GetShoppingCart(customerId);

			if (order == null)
			{
				throw new InvalidDataException("Order not found in db!");
			}

			contact.Person.Validate();
			contact.Address.Validate();

			var person = UpsertEntity(contact.Person, _unitOfWork.PersonRepository);
			var address = UpsertEntity(contact.Address, _unitOfWork.AddressRepository);
			if (person == null) throw new ArgumentNullException("Error storing person");
			if (address == null) throw new ArgumentNullException("Error storing address");

			contact.AddressId = address.Id;
			contact.PersonId = person.Id;

			contact = UpsertEntity(contact, _unitOfWork.ContactRepository);
			order.BillingContactId = contact.Id;

			_unitOfWork.OrderRepository.Detach(order);
			if (order.BillingContact != null)
			{
				_unitOfWork.ContactRepository.Detach(order.BillingContact);
			}
			_unitOfWork.OrderRepository.Update(order);
			_unitOfWork.OrderRepository.Save();

			return true;
		}

		#endregion

		#region Products
		private bool CreateProductToOrderRelation(int productId, int count, Order order)
		{
			if (count <= 0) throw new InvalidDataException("Count must be higher than 0");
			var relation = _unitOfWork.OrderProductRepository.GetOrderProduct(productId, order.Id);

			if (relation != null)
			{
				relation.ProductId = productId;
				relation.OrderId = order.Id;
				relation.Count = relation.Count + count;

				_unitOfWork.OrderProductRepository.Update(relation, true);
			}
			else
			{
				var product = new OrderProduct
				{
					ProductId = productId,
					OrderId = order.Id,
					Count = count
				};
				_unitOfWork.OrderProductRepository.Add(product, true);
			}

			return true;
		}

		public bool AddProductToOrder(OrderProduct product)
		{
			if (product == null) throw new ArgumentNullException("Order product is null");
			product.Validate();

			var selectedOrder = GetOrder(product.OrderId);
			if (selectedOrder == null) throw new InvalidDataException("Order not found");

			return CreateProductToOrderRelation(product.ProductId, product.Count, selectedOrder);
		}

		public bool AddProductToOrder(int productId, int customerId, int count)
		{
			if (!_unitOfWork.ProductRepository.IsStored(productId)) throw new InvalidDataException("Product not found");

			var cart = GetShoppingCart(customerId);
			return CreateProductToOrderRelation(productId, count, cart);
		}

		public bool RemoveProductFromOrder(int productId, int customerId)
		{
			var cart = GetShoppingCart(customerId);

			var product = _unitOfWork.OrderProductRepository.GetOrderProduct(productId, cart.Id);
			if (product == null) throw new InvalidDataException("Product not found");

			_unitOfWork.OrderProductRepository.Remove(product);
			_unitOfWork.OrderProductRepository.Save();

			return true;
		}

		#endregion
	}
}
