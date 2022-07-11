using DomainModels.Enum;
using DomainModels.ViewModel.Order;

namespace Services
{
    public interface IOrderService
    {
        BaseOrderViewModel GetById(int orderId);
        BaseOrderViewModel GetByTableNumber(int tableNumber);
        PlacedOrderViewModel PlaceOrder(int tableNumber);
        CompletedOrderViewModel Complete(int tableNumber);
        BaseOrderViewModel AddChefRecommended(int tableNumber, Category category);
        BaseOrderViewModel AddOrderItem(int tableNumber, int menuId, int quantity);
        void CancelOrderItem(int orderItemId);
        BaseOrderViewModel AddAllChefRecommendation(int tableNumber, int quantity);
    }
}
