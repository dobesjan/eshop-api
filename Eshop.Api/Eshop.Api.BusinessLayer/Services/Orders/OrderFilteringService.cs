using Eshop.Api.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.BusinessLayer.Services.Orders
{
	public partial class OrderService : EshopService, IOrderService
	{
		private Expression<Func<Order, bool>> GetOrdersFilteringExpression(OrderFilter filter)
		{
			if (filter == null) throw new ArgumentNullException(nameof(filter));

			var parameter = Expression.Parameter(typeof(Order), "o");
			var expressions = new List<Expression>();

			if (filter.OrderId.HasValue)
				expressions.Add(Expression.Equal(Expression.Property(parameter, "Id"), Expression.Constant(filter.OrderId.Value)));

			if (!string.IsNullOrEmpty(filter.OrderStatus))
				expressions.Add(Expression.Call(Expression.Property(Expression.Property(parameter, "OrderStatus"), "Name"), "Contains", null, Expression.Constant(filter.OrderStatus)));

			if (filter.IsOrdered.HasValue)
				expressions.Add(Expression.Equal(Expression.Property(parameter, "IsOrdered"), Expression.Constant(filter.IsOrdered.Value)));

			if (filter.CreatedDateFrom.HasValue)
				expressions.Add(Expression.GreaterThanOrEqual(Expression.Property(parameter, "CreatedDate"), Expression.Constant(filter.CreatedDateFrom.Value)));

			if (filter.CreatedDateTo.HasValue)
				expressions.Add(Expression.LessThanOrEqual(Expression.Property(parameter, "CreatedDate"), Expression.Constant(filter.CreatedDateTo.Value)));

			if (filter.SentTimeFrom.HasValue)
				expressions.Add(Expression.GreaterThanOrEqual(Expression.Property(parameter, "SentTime"), Expression.Constant(filter.SentTimeFrom.Value)));

			if (filter.SentTimeTo.HasValue)
				expressions.Add(Expression.LessThanOrEqual(Expression.Property(parameter, "SentTime"), Expression.Constant(filter.SentTimeTo.Value)));

			if (filter.DeliveryTimeFrom.HasValue)
				expressions.Add(Expression.GreaterThanOrEqual(Expression.Property(parameter, "DeliveryTime"), Expression.Constant(filter.DeliveryTimeFrom.Value)));

			if (filter.DeliveryTimeTo.HasValue)
				expressions.Add(Expression.LessThanOrEqual(Expression.Property(parameter, "DeliveryTime"), Expression.Constant(filter.DeliveryTimeTo.Value)));

			if (!string.IsNullOrEmpty(filter.Shipping))
				expressions.Add(Expression.Call(Expression.Property(Expression.Property(parameter, "Shipping"), "Name"), "Contains", null, Expression.Constant(filter.Shipping)));

			if (!string.IsNullOrEmpty(filter.FirstName))
				expressions.Add(Expression.Call(Expression.Property(Expression.Property(Expression.Property(parameter, "Customer"), "Person"), "FirstName"), "Contains", null, Expression.Constant(filter.FirstName)));

			if (!string.IsNullOrEmpty(filter.LastName))
				expressions.Add(Expression.Call(Expression.Property(Expression.Property(Expression.Property(parameter, "Customer"), "Person"), "LastName"), "Contains", null, Expression.Constant(filter.LastName)));

			if (!string.IsNullOrEmpty(filter.Email))
				expressions.Add(Expression.Call(Expression.Property(Expression.Property(Expression.Property(parameter, "Customer"), "Person"), "Email"), "Contains", null, Expression.Constant(filter.Email)));

			if (!string.IsNullOrEmpty(filter.PhoneNumber))
				expressions.Add(Expression.Call(Expression.Property(Expression.Property(Expression.Property(parameter, "Customer"), "Person"), "PhoneNumber"), "Contains", null, Expression.Constant(filter.PhoneNumber)));

			if (!string.IsNullOrEmpty(filter.Street))
				expressions.Add(Expression.Call(Expression.Property(Expression.Property(Expression.Property(parameter, "Customer"), "Address"), "Street"), "Contains", null, Expression.Constant(filter.Street)));

			if (!string.IsNullOrEmpty(filter.City))
				expressions.Add(Expression.Call(Expression.Property(Expression.Property(Expression.Property(parameter, "Customer"), "Address"), "City"), "Contains", null, Expression.Constant(filter.City)));

			if (!string.IsNullOrEmpty(filter.PostalCode))
				expressions.Add(Expression.Call(Expression.Property(Expression.Property(Expression.Property(parameter, "Customer"), "Address"), "PostalCode"), "Contains", null, Expression.Constant(filter.PostalCode)));

			if (!string.IsNullOrEmpty(filter.Country))
				expressions.Add(Expression.Call(Expression.Property(Expression.Property(Expression.Property(parameter, "Customer"), "Address"), "Country"), "Contains", null, Expression.Constant(filter.Country)));

			if (filter.NewsletterAgree.HasValue)
				expressions.Add(Expression.Equal(Expression.Property(Expression.Property(parameter, "Customer"), "NewsletterAgree"), Expression.Constant(filter.NewsletterAgree.Value)));

			if (!expressions.Any())
			{
				return null;
			}

			var body = expressions.Aggregate(Expression.AndAlso);
			return Expression.Lambda<Func<Order, bool>>(body, parameter);
		}

		public IEnumerable<Order> GetOrdersByFilter(OrderFilter filter, int offset = 0, int limit = 0)
		{
			var expression = GetOrdersFilteringExpression(filter);

			if (expression == null)
			{
				return _unitOfWork.OrderRepository.GetOrders(offset: offset, limit: limit);
			}

			return _unitOfWork.OrderRepository.GetOrders(expression, offset, limit);
		}

		public int GetOrdersByFilterCount(OrderFilter filter)
		{
			var expression = GetOrdersFilteringExpression(filter);

			if (expression == null)
			{
				return _unitOfWork.OrderRepository.Count();
			}

			return _unitOfWork.OrderRepository.Count(expression);
		}
	}
}
