using System;
using System.Collections.Generic;
using System.Linq;
using DomainModels.Constants;
using DomainModels.Extensions;
using DomainModels.ViewModel.OrderItem;

namespace DomainModels.ViewModel.Order
{
    public abstract class BaseOrderViewModel
    {
        private int id;
        public int Id
        {
            get => this.id;
            set => this.id = value.MustBeGreaterThanZero(ErrorMessages.OrderIdInvalid);
        }

        private int tableNumber;
        public int TableNumber
        {
            get => this.tableNumber;
            set => this.tableNumber = value.MustBeGreaterThanZero(ErrorMessages.OrderTableNumberInvalid);
        }

        private DateTime date;
        public DateTime Date
        {
            get => this.date;
            set => this.date = value.MustBeLessThan(DateTime.UtcNow, ErrorMessages.OrderDateInvalid);
        }

        private IEnumerable<BaseOrderItemViewModel> orderItems;
        public IEnumerable<BaseOrderItemViewModel> OrderItems
        {
            get => this.orderItems;
            set => this.orderItems = value.MustNotBeNull(ErrorMessages.OrderItemsInvalid).ToList();
        }

        protected decimal BasePrice { get => this.OrderItems.Sum(_ => _.TotalPrice); }

        public abstract OrderStatus Status { get; }
    }
}
