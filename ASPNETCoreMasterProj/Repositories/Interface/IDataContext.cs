using System.Collections.Generic;
using System.Linq;
using DomainModels.Entity;

namespace Repositories.Interface
{
    public interface IDataContext
    {
        IQueryable<TEntity> GetAll<TEntity>() where TEntity : EntityBase;
        TEntity GetById<TEntity>(int id) where TEntity : EntityBase;
        void AddEntity<TEntity>(TEntity entity) where TEntity : EntityBase;
        void UpdateEntity<TEntity>(TEntity entity) where TEntity : EntityBase;
        void DeleteEntity<TEntity>(int id) where TEntity : EntityBase;
        void UpdateAllEntity<TEntity>(IEnumerable<TEntity> entity) where TEntity : EntityBase;
    }
}
