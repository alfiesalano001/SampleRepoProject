using DomainModels.Entity;
using Repositories.Interface;

namespace Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Order GetByTableNumber(int tableNumber);
        OrderItem GetOrderItemById(int orderId);
        void DeleteOrderItem(int orderId);
    }
}
