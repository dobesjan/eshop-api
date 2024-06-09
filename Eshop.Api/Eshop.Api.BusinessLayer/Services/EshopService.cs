using Eshop.Api.DataAccess.Repository;
using Eshop.Api.Models;
using Eshop.Api.Models.Contacts;
using Eshop.Api.Models.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.BusinessLayer.Services
{
    public class EshopService : IEshopService
	{

		public T UpsertEntity<T>(T entity, IRepository<T> repository) where T : Entity
		{
			if (entity != null)
			{
				if (entity.Id > 0)
				{
					if (!repository.IsStored(entity.Id))
					{
						throw new InvalidDataException($"{nameof(entity)} with id: {entity.Id} not found in db!");
					}

					entity = repository.Update(entity, true);
                }
				else
				{
					entity = repository.Add(entity, true);
				}

				return entity;
			}
			else
			{
				throw new ArgumentNullException($"{nameof(entity)} with id: {entity.Id} is null!");
			}
		}
	}
}
