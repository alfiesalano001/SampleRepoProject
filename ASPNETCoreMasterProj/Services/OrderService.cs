using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DomainModels.Constants;
using DomainModels.Entity;
using DomainModels.Enum;
using DomainModels.Exceptions;
using DomainModels.Extensions;
using DomainModels.Helpers;
using DomainModels.ViewModel.Order;
using Microsoft.Extensions.Logging;
using Repositories;
using Repositories.Interface;
using Services.Extensions;

namespace Services
{
    public class OrderService : ServiceBase<OrderService>, IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMenuRepository menuRepository;
        private readonly IGenericRepository<Stock> stockRepository;

        public OrderService(IMapper mapper, ILogger<OrderService> logger,
            IOrderRepository orderRepository, IMenuRepository menuRepository, IGenericRepository<Stock> stockRepository)
            : base(mapper, logger)
        {
            this.orderRepository = orderRepository.MustBeImplemented();
            this.menuRepository = menuRepository.MustBeImplemented();
            this.stockRepository = stockRepository.MustBeImplemented();
        }

        public BaseOrderViewModel GetById(int orderId)
        {
            Throw<BadRequestException>.IfNotGreaterThanZero(orderId, ErrorMessages.OrderIdInvalid);

            var order = this.orderRepository.GetById(orderId).MustNotBeNull(ErrorMessages.NotFound.OrderError);

            var menuItems = this.menuRepository.GetAll();

            return order.MustNotBeNull(ErrorMessages.NotFound.OrderError).CreateOrderViewModel(menuItems);
        }

        public BaseOrderViewModel GetByTableNumber(int tableNumber)
        {
            Throw<BadRequestException>.IfNotGreaterThanZero(tableNumber, ErrorMessages.OrderTableNumberInvalid);

            var order = this.orderRepository.GetByTableNumber(tableNumber).MustNotBeNull(ErrorMessages.NotFound.OrderError);

            return GetById(order.Id);
        }

        public PlacedOrderViewModel PlaceOrder(int tableNumber)
        {
            _logger.LogInformation($"OrderService -> CreatingOrders: {tableNumber}");

            Throw<BadRequestException>.IfNotGreaterThanZero(tableNumber, ErrorMessages.Operations.PlaceOrderTableNumberInvalidError);

            var order = this.orderRepository.GetByTableNumber(tableNumber);

            Throw<NotFoundException>.IfNull(order, ErrorMessages.NotFound.OrderError);
            Throw<BadRequestException>.IfFalse(order.OrderItems.Any(_ => !_.IsPlaced), ErrorMessages.Operations.PlaceOrderEmptyPendingItemsError);

            IEnumerable<Stock> stocks = this.stockRepository.GetAll();
            var menuItems = this.menuRepository.GetAll();

            foreach (var item in order.OrderItems)
            {
                int counter = 1;
                var menuResult = menuItems.FirstOrDefault(x => x.Id == item.MenuId);
                var stocksUpdate = new List<Stock>();

                foreach (var result in menuResult.Ingredients)
                {
                    var stockResult = stocks.FirstOrDefault(x => x.Id == result.StockId && x.Quantity >= result.Quantity);
                    if (stockResult == null)
                        break;

                    stockResult.Quantity -= result.Quantity;
                    stocksUpdate.Add(stockResult);

                    if (item.IsPlaced)
                        break;                    
                    else if (counter == menuResult.Ingredients.Count())
                    {
                        item.IsPlaced = true;
                        item.DatePlaced = DateTime.UtcNow;
                        
                        this.stockRepository.UpdateAllEntity(stocksUpdate);
                    }
                    counter++;
                }
            }

            this.orderRepository.Update(order);

            return order.MustNotBeNull(ErrorMessages.NotFound.OrderError).CreatePlacedOrder(menuItems);
        }

        public BaseOrderViewModel AddOrderItem(int tableNumber, int menuId, int quantity)
        {
            _logger.LogInformation($"OrderService -> AddOrderItem: {tableNumber}, {menuId}, {quantity}");

            Throw<BadRequestException>.IfNotGreaterThanZero(menuId, ErrorMessages.Operations.AddOrderMenuIdInvalidError);
            Throw<BadRequestException>.IfNotGreaterThanZero(quantity, ErrorMessages.Operations.AddOrderQuantityInvalidError);

            var order = this.TryGetOrderByTable(tableNumber).MustNotBeNull(ErrorMessages.NotFound.OrderError);
            var menu = this.menuRepository.GetById(menuId).MustNotBeNull(ErrorMessages.NotFound.MenuError);

            this.AddMenuToExistingOrder(menu, order, quantity);

            var menuItems = this.menuRepository.GetAll();

            return order.MustNotBeNull(ErrorMessages.NotFound.OrderError).CreateOrderViewModel(menuItems);
        }

        public BaseOrderViewModel AddChefRecommended(int tableNumber, Category category)
        {
            var menu = this.menuRepository.GetChefRecommendedByCategory(category).MustNotBeNull(ErrorMessages.NotFound.ChefRecommendedCategoryError);

            return AddOrderItem(tableNumber, menu.Id, 1);
        }

        public BaseOrderViewModel AddAllChefRecommendation(int tableNumber, int quantity)
        {
            _logger.LogInformation("OrderService -> AddAllChefRecommendation");

            var menu = this.menuRepository.GetAllChefRecommended().MustNotBeNullOrEmpty(ErrorMessages.NotFound.AllChefRecommendationsError);

            var order = this.TryGetOrderByTable(tableNumber);

            foreach (var item in menu)
            {
                this.AddMenuToExistingOrder(item, order, quantity);
            }

            var menuItems = this.menuRepository.GetAll();

            return order.MustNotBeNull(ErrorMessages.NotFound.OrderError).CreateOrderViewModel(menuItems);
        }

        public CompletedOrderViewModel Complete(int tableNumber)
        {
            var order = this.orderRepository.GetByTableNumber(tableNumber).MustNotBeNull(ErrorMessages.NotFound.OrderError);

            var menuItems = this.menuRepository.GetAll();

            Throw<BadRequestException>.IfFalse(order.OrderItems.All(_ => _.IsPlaced), ErrorMessages.Operations.CompleteOrderPendingOrdersError);
            Throw<BadRequestException>.IfFalse(order.OrderItems.All(_ => _.IsServed(menuItems)), ErrorMessages.Operations.CompleteOrderNotServedError);

            order.IsCompleted = true;

            this.orderRepository.Update(order);

            return order.MustNotBeNull(ErrorMessages.NotFound.OrderError).CreateCompletedOrder(menuItems);
        }

        public void CancelOrderItem(int orderItemId)
        {
            _logger.LogInformation("OrderService -> CancelOrderItem");

            Throw<BadRequestException>.IfNotGreaterThanZero(orderItemId, ErrorMessages.Operations.CancelOrderItemIdInvalid);

            var orderItem = this.orderRepository.GetOrderItemById(orderItemId).MustNotBeNull(ErrorMessages.NotFound.OrderItemError);

            Throw<BadRequestException>.IfTrue(orderItem.IsPlaced, ErrorMessages.Operations.CancelPlacedOrderError);

            this.orderRepository.DeleteOrderItem(orderItemId);
        }

        private void AddMenuToExistingOrder(Menu menu, Order order, int quantity)
        {
            Throw<NotFoundException>.IfNull(order, ErrorMessages.NotFound.OrderError);
            Throw<NotFoundException>.IfNull(menu, ErrorMessages.NotFound.MenuError);
            Throw<BadRequestException>.IfNotGreaterThanZero(quantity, ErrorMessages.Operations.AddOrderQuantityInvalidError);

            var orderItem = order.OrderItems.FirstOrDefault(i => i.OrderId == order.Id && i.MenuId == menu.Id && !i.IsPlaced);

            if (orderItem != null)
            {
                orderItem.Quantity += quantity;
            }
            else
            {
                order.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    MenuId = menu.Id,
                    Quantity = quantity,
                    IsPlaced = false,
                    DatePlaced = null
                });
            }

            this.orderRepository.Update(order);
        }

        private Order TryGetOrderByTable(int tableNumber)
        {
            Throw<BadRequestException>.IfNotGreaterThanZero(tableNumber, ErrorMessages.OrderTableNumberInvalid);

            var order = this.orderRepository.GetByTableNumber(tableNumber) ?? CreateNewOrder(tableNumber);

            return order;
        }

        private Order CreateNewOrder(int tableNumber)
        {
            var order = new Order() { TableNumber = tableNumber, Date = DateTime.UtcNow };

            this.orderRepository.Add(order);

            return order;
        }
    }
}
