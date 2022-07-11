using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainModels.Entity;
using DomainModels.Exceptions;
using DomainModels.Helpers;
using Microsoft.Extensions.Logging;
using Repositories.Interface;

namespace Repositories.Decorators
{
    public class DataContextDecorator : IDataContext
    {
        private readonly IDataContext dataContext;
        private readonly ILogger<DataContextDecorator> logger;

        public DataContextDecorator(IDataContext dataContext, ILogger<DataContextDecorator> logger)
        {
            this.dataContext = dataContext;
            this.logger = logger;
        }

        public void AddEntity<TEntity>(TEntity entity) where TEntity : EntityBase
        {
            Throw<BadRequestException>.IfNull(entity, "");

            try
            {
                this.dataContext.AddEntity(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteEntity<TEntity>(int id) where TEntity : EntityBase
        {
            try
            {
                this.dataContext.DeleteEntity<TEntity>(id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IQueryable<TEntity> GetAll<TEntity>() where TEntity : EntityBase
        {
            try
            {
                logger.LogInformation("Decorator Get All");

                return this.dataContext.GetAll<TEntity>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TEntity GetById<TEntity>(int id) where TEntity : EntityBase
        {
            try
            {
                return this.dataContext.GetById<TEntity>(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateEntity<TEntity>(TEntity entity) where TEntity : EntityBase
        {
            try
            {
                this.dataContext.UpdateEntity(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateAllEntity<TEntity>(IEnumerable<TEntity>  entity) where TEntity : EntityBase
        {
            try
            {
                this.dataContext.UpdateAllEntity(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
