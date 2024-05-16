using Eshop.Api.DataAccess.Repository.Interfaces;
using Eshop.Api.Models;
using Eshop.Api.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.BusinessLayer.Services.Interfaces
{
	public interface IEshopService
	{
		bool UpsertEntity<T>(T entity, IRepository<T> repository) where T : Entity;
	}
}
