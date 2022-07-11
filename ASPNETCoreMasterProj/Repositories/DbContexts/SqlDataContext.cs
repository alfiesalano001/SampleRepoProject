using System.Collections.Generic;
using System.Linq;
using DomainModels.Entity;
using Microsoft.EntityFrameworkCore;
using Repositories.Interface;

namespace Repositories.DbContexts
{
    public sealed class SqlDataContext : DbContext, IDataContext
    {
        public SqlDataContext(DbContextOptions<SqlDataContext> options)
            : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public IQueryable<TEntity> GetAll<TEntity>() where TEntity : EntityBase => this.Set<TEntity>();

        public TEntity GetById<TEntity>(int id) where TEntity : EntityBase => this.Set<TEntity>().FirstOrDefault(_ => _.Id == id);

        public void AddEntity<TEntity>(TEntity entity) where TEntity : EntityBase
        {
            this.Set<TEntity>().Add(entity);

            //TODO: Handle Error
            this.SaveChanges();
        }

        public void DeleteEntity<TEntity>(int id) where TEntity : EntityBase
        {
            var entity = GetById<TEntity>(id);

            this.Set<TEntity>().Remove(entity);

            //TODO: Handle Error
            this.SaveChanges();
        }

        public void UpdateEntity<TEntity>(TEntity entity) where TEntity : EntityBase
        {
            this.Attach(entity);

            this.Entry(entity).State = EntityState.Modified;

            //TODO: Handle Error
            this.SaveChanges();
        }

        public void UpdateAllEntity<TEntity>(IEnumerable<TEntity> entity) where TEntity : EntityBase
        {
            this.Attach(entity);

            this.Entry(entity).State = EntityState.Modified;

            //TODO: Handle Error
            this.SaveChanges();
        }
    }
}
