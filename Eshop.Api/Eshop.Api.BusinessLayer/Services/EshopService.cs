using Eshop.Api.BusinessLayer.Services.Interfaces;
using Eshop.Api.DataAccess.Repository;
using Eshop.Api.DataAccess.Repository.Interfaces;
using Eshop.Api.Models;
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

		public bool UpsertEntity<T>(T entity, IRepository<T> repository) where T : Entity
		{
			if (entity != null)
			{
				if (entity.Id > 0)
				{
					if (!repository.IsStored(entity.Id))
					{
						throw new InvalidDataException($"{nameof(entity)} with id: {entity.Id} not found in db!");
					}

					repository.Update(entity);
					repository.Save();
				}
				else
				{
					repository.Add(entity);
					repository.Save();
				}

				return true;
			}
			else
			{
				throw new ArgumentNullException($"{nameof(entity)} with id: {entity.Id} is null!");
			}
		}
	}
}
