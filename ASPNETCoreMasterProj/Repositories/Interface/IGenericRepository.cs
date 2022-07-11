using System.Collections.Generic;
using System.Linq;
using DomainModels.Entity;

namespace Repositories.Interface
{
    public interface IGenericRepository<TEntity> where TEntity : EntityBase
    {
        IQueryable<TEntity> GetAll();
        TEntity GetById(int id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(int id);
        void UpdateAllEntity(IEnumerable<TEntity> entity);
    }
}
