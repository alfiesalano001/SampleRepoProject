using System;
using System.Collections.Generic;
using System.Linq;
using DomainModels.Constants;
using DomainModels.Entity;
using DomainModels.Exceptions;
using DomainModels.Helpers;
using DomainModels.ViewModel.Menu;
using DomainModels.ViewModel.Order;
using DomainModels.ViewModel.OrderItem;

namespace Services.Extensions
{
    public static class EntityExtensions
    {
        /// <summary>
        /// Returns a placed instance of order if order has not been completed. Otherwise, returns a completed instance.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="menuItems"></param>
        /// <returns></returns>
        public static BaseOrderViewModel CreateOrderViewModel(this Order order, IEnumerable<Menu> menuItems)
        {
            Throw<BadRequestException>.IfNull(order, ErrorMessages.NotFound.OrderError);

            return order.IsCompleted ? order.CreateCompletedOrder(menuItems) : order.CreatePlacedOrder(menuItems);
        }

        /// <summary>
        /// Returns a placed instance of an order
        /// </summary>
        /// <param name="order"></param>
        /// <param name="menuItems"></param>
        /// <returns></returns>
        public static PlacedOrderViewModel CreatePlacedOrder(this Order order, IEnumerable<Menu> menuItems)
        {
            Throw<BadRequestException>.IfNull(order, ErrorMessages.NotFound.OrderError);

            var orderItems = order.OrderItems.Select(_ => _.CreateOrderItemViewModel(_.GetMenu(menuItems)));

            return new PlacedOrderViewModel
            {
                Id = order.Id,
                TableNumber = order.TableNumber,
                Date = order.Date,
                OrderItems = orderItems
            };
        }

        /// <summary>
        /// Returns the menu of the order item based on the given menu items.
        /// </summary>
        /// <param name="orderItem"></param>
        /// <param name="menuItems"></param>
        /// <returns></returns>
        public static Menu GetMenu(this OrderItem orderItem, IEnumerable<Menu> menuItems)
        {
            return menuItems.FirstOrDefault(_ => _.Id == orderItem.MenuId);
        }

        /// <summary>
        /// Returns a completed instance of an order
        /// </summary>
        /// <param name="order"></param>
        /// <param name="menuItems"></param>
        /// <returns></returns>
        public static CompletedOrderViewModel CreateCompletedOrder(this Order order, IEnumerable<Menu> menuItems)
        {
            Throw<BadRequestException>.IfNull(order, ErrorMessages.NotFound.OrderError);

            var orderItems = order.OrderItems.Select(_ => _.CreateCompletedOrderItemViewModel(_.GetMenu(menuItems)));

            return new CompletedOrderViewModel
            {
                Id = order.Id,
                TableNumber = order.TableNumber,
                Date = order.Date,
                OrderItems = orderItems
            };
        }

        /// <summary>
        /// Returns true if the order item has been placed with corresponding date.
        /// </summary>
        /// <param name="orderItem"></param>
        /// <param name="menuItems"></param>
        /// <returns></returns>
        public static bool IsServed(this OrderItem orderItem, IEnumerable<Menu> menuItems)
        {
            var menu = orderItem.GetMenu(menuItems);

            Throw<BadRequestException>.IfNull(menu, ErrorMessages.NotFound.OrderItemMenuError);
            Throw<BadRequestException>.IfTrue(orderItem.IsPlaced && !orderItem.DatePlaced.HasValue, "Order item has been placed but has a missing date placed value.");

            if (!orderItem.IsPlaced) return false;

            var totalTimeToServe = (menu.PrepTimeInSec + menu.CookTimeInSec) * orderItem.Quantity;

            return DateTime.UtcNow > orderItem.DatePlaced.Value.AddSeconds(totalTimeToServe);
        }

        /// <summary>
        /// Returns a placed order item
        /// </summary>
        /// <param name="orderItem"></param>
        /// <param name="menu"></param>
        /// <returns></returns>
        public static BaseOrderItemViewModel CreateOrderItemViewModel(this OrderItem orderItem, Menu menu)
        {
            Throw<BadRequestException>.IfNull(orderItem, ErrorMessages.NotFound.OrderItemError);
            Throw<BadRequestException>.IfNull(menu, ErrorMessages.NotFound.OrderItemMenuError);

            if (orderItem.IsPlaced && orderItem.DatePlaced.HasValue) return orderItem.CreatePlacedOrderItem(menu);

            return orderItem.CreatePendingOrderItem(menu);
        }

        /// <summary>
        /// Returns a completed order item
        /// </summary>
        /// <param name="orderItem"></param>
        /// <param name="menu"></param>
        /// <returns></returns>
        public static CompletedOrderItemViewModel CreateCompletedOrderItemViewModel(this OrderItem orderItem, Menu menu)
        {
            Throw<BadRequestException>.IfNull(orderItem, ErrorMessages.NotFound.OrderItemError);
            Throw<BadRequestException>.IfNull(menu, ErrorMessages.NotFound.CompletedOrderItemMenuError);

            return new CompletedOrderItemViewModel
            {
                Id = orderItem.Id,
                Quantity = orderItem.Quantity,
                Menu = menu.CreateMenuDetails()
            };
        }

        /// <summary>
        /// Returns a pending order item that has yet to be placed as an order
        /// </summary>
        /// <param name="orderItem"></param>
        /// <param name="menu"></param>
        /// <returns></returns>
        public static PendingOrderItemViewModel CreatePendingOrderItem(this OrderItem orderItem, Menu menu)
        {
            Throw<BadRequestException>.IfNull(orderItem, ErrorMessages.NotFound.OrderItemError);
            Throw<BadRequestException>.IfNull(menu, ErrorMessages.NotFound.PendingOrderItemMenuError);

            return new PendingOrderItemViewModel
            {
                Id = orderItem.Id,
                Quantity = orderItem.Quantity,
                Menu = menu.CreateMenuDetails()
            };
        }

        /// <summary>
        /// Returns a placed order item instance
        /// </summary>
        /// <param name="orderItem"></param>
        /// <param name="menu"></param>
        /// <returns></returns>
        public static PlacedOrderItemViewModel CreatePlacedOrderItem(this OrderItem orderItem, Menu menu)
        {
            Throw<BadRequestException>.IfNull(orderItem, ErrorMessages.NotFound.OrderItemError);
            Throw<BadRequestException>.IfNull(menu, ErrorMessages.NotFound.PlacedOrderItemMenuError);

            return new PlacedOrderItemViewModel
            {
                Id = orderItem.Id,
                Quantity = orderItem.Quantity,
                DatePlaced = orderItem.DatePlaced.Value,
                Menu = menu.CreateMenuDetails()
            };
        }

        /// <summary>
        /// Returns a menu details instance
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public static MenuDetailsViewModel CreateMenuDetails(this Menu menu)
        {
            Throw<BadRequestException>.IfNull(menu, ErrorMessages.NotFound.MenuError);

            return new MenuDetailsViewModel
            {
                Id = menu.Id,
                Category = menu.Category,
                ChefRecommendation = menu.ChefRecommendation,
                CookTimeInSec = menu.CookTimeInSec,
                Description = menu.Description,
                ItemPrice = menu.ItemPrice,
                Name = menu.Name,
                PrepTimeInSec = menu.PrepTimeInSec
            };
        }
    }
}
