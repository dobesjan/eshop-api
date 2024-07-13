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
		#region Payment
		private void CheckIfShippingSupportsPaymentMethod(int paymentId, int orderId)
		{
			var order = GetOrder(orderId);
			if (!order.ShippingId.HasValue) throw new InvalidDataException("Shipping method not provided");
			var shipping = _unitOfWork.ShippingRepository.GetEnabledShipping(order.ShippingId.Value);
			if (shipping != null
				&& shipping.ShippingPaymentMethod != null
				&& shipping.ShippingPaymentMethod.Exists(sp => sp.PaymentMethodId == paymentId)) throw new InvalidDataException("Payment not supported in provided shipping");
		}

		//TODO: Consider if it's worth to have method(s) like this
		public bool UpsertOrderPayment(Payment payment, bool recalculate = false)
		{
			if (!_unitOfWork.PaymentMethodRepository.IsPaymentMethodEnabled(payment.PaymentMethodId)) throw new InvalidDataException("Payment method not supported");
			if (!_unitOfWork.PaymentStatusRepository.IsStored(payment.PaymentStatusId)) throw new InvalidDataException($"Wrong payment status with id {payment.PaymentStatusId}");

			CheckIfShippingSupportsPaymentMethod(payment.Id, payment.OrderId);

			if (recalculate)
			{
				var order = GetOrder(payment.OrderId);
				payment.Cost = order.CalculateTotalCost(payment.CurrencyId);
				payment.CostWithTax = order.CalculateTotalCost(payment.CurrencyId, true);
			}
			else
			{
				CheckOrderIsStored(payment.OrderId);
			}

			return UpsertEntity(payment, _unitOfWork.PaymentRepository) != null;
		}

		public bool GeneratePayment(int orderId, int paymentMethodId)
		{
			if (!_unitOfWork.PaymentMethodRepository.IsPaymentMethodEnabled(paymentMethodId)) throw new InvalidDataException("Payment method not supported");
			//TODO: Consider how to handle payment statuses
			var paymentStatusId = 1;
			if (!_unitOfWork.PaymentStatusRepository.IsStored(paymentStatusId)) throw new InvalidDataException($"Wrong payment status with id {paymentStatusId}");
			//CheckIfShippingSupportsPaymentMethod(paymentMethodId, orderId);

			var order = GetOrder(orderId);
			var cost = order.CalculateTotalCost();
			var costWithTax = order.CalculateTotalCost(true);

			if (order.Payment != null)
			{
				_unitOfWork.PaymentRepository.Remove(order.Payment);
			}

			var payment = new Payment
			{
				OrderId = orderId,
				PaymentStatusId = paymentStatusId,
				PaymentMethodId = paymentMethodId,
				Cost = cost,
				CostWithTax = costWithTax
			};

			_unitOfWork.PaymentRepository.Add(payment);
			_unitOfWork.PaymentRepository.Save();
			return true;
		}

		public IEnumerable<PaymentMethod> GetPaymentMethodsForShipping(int shippingId)
		{
			return _unitOfWork.PaymentMethodRepository.GetPaymentMethodsForRepository(shippingId);
		}

		#endregion
	}
}
