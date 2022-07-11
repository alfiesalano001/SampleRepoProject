using System;
using System.Collections.Generic;
using System.Linq;
using DomainModels.ViewModel.OrderItem;
using DomainModels.Constants;
using DomainModels.Extensions;

namespace DomainModels.ViewModel.Order
{
    public class PlacedOrderViewModel : BaseOrderViewModel
    {
        private const decimal SERVICE_CHARGE_PERCENT = 0.10m;
        private const decimal INCLUSIVE_TAX_PERCENT = 0.12m;

        public decimal ServiceCharge { get => base.BasePrice * SERVICE_CHARGE_PERCENT; }
        public decimal InclusiveTax { get => base.BasePrice * INCLUSIVE_TAX_PERCENT; }
        public decimal TotalBill { get => base.BasePrice + ServiceCharge; }

        /// <summary>
        /// Calculate the status based on the remaining time and cooking time
        /// </summary>
        public override OrderStatus Status
        {
            get
            {
                if (RemainingPreparationTime > 0) return OrderStatus.Preparing;
                if (RemainingCookingTime > 0) return OrderStatus.Cooking;

                return OrderStatus.Served;
            }
        }

        /// <summary>
        /// Calculate the remaining time of preparation time excluding items not yet placed
        /// </summary>
        public int RemainingPreparationTime
        {
            get
            {
                var orderItems = this.OrderItems.OfType<PlacedOrderItemViewModel>();

                if (!orderItems.Any()) return default;

                return orderItems.Max(_ => _.RemainingPreparationTime);
            }
        }

        /// <summary>
        /// Calculate the remaining time of cooking time excluding items not yet placed
        /// </summary>
        public int RemainingCookingTime
        {
            get
            {
                var orderItems = this.OrderItems.OfType<PlacedOrderItemViewModel>();

                if (!orderItems.Any()) return default;

                return orderItems.Max(_ => _.RemainingCookingTime);
            }
        }
    }
}
