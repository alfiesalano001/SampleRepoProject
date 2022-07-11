using System.Collections.Generic;
using System.Linq;
using DomainModels.Entity;
using DomainModels.Extensions;
using Repositories.Interface;

namespace Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> 
        where TEntity : EntityBase
    {
        protected IDataContext dbContext;

        public GenericRepository(IDataContext dbContext) => this.dbContext = dbContext.MustBeImplemented();

        public virtual IQueryable<TEntity> GetAll() => this.dbContext.GetAll<TEntity>();

        public virtual TEntity GetById(int id) => this.dbContext.GetById<TEntity>(id);

        public virtual void Add(TEntity entity) => this.dbContext.AddEntity(entity);

        public virtual void Delete(int id) => this.dbContext.DeleteEntity<TEntity>(id);

        public virtual void Update(TEntity entity) => this.dbContext.UpdateEntity(entity);
        public virtual void UpdateAllEntity(IEnumerable<TEntity> entity) => this.dbContext.UpdateAllEntity(entity);
    }

}
