using Eshop.Api.DataAccess.Repository;
using Eshop.Api.Models;
using Eshop.Api.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.BusinessLayer.Services
{
    public interface IEshopService
    {
        T UpsertEntity<T>(T entity, IRepository<T> repository) where T : Entity;
        //bool LinkEntity<T, A, B>(T linkingEntity, int entityAId, int entityBId, IRepository<A> repositoryA, IRepository<B> repositoryB) where T : Entity where A : Entity where B : Entity;
    }
}
