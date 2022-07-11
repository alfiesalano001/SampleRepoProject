using System.Linq;
using DomainModels.Constants;
using DomainModels.Entity;
using DomainModels.Extensions;
using Microsoft.EntityFrameworkCore;
using Repositories.Interface;

namespace Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(IDataContext dBContext)
            : base(dBContext) { }

        public override Order GetById(int id) => GetAll().Include(_ => _.OrderItems)
                                                         .FirstOrDefault(_ => _.Id == id);

        public Order GetByTableNumber(int tableNumber) => GetAll().Include(_ => _.OrderItems)
                                                                  .OrderByDescending(_ => _.Date)
                                                                  .FirstOrDefault(_ => _.TableNumber == tableNumber && !_.IsCompleted);

        public OrderItem GetOrderItemById(int orderItemId) => base.dbContext.GetById<OrderItem>(orderItemId);

        public void DeleteOrderItem(int orderItemId) => base.dbContext.DeleteEntity<OrderItem>(orderItemId);
    }
}
