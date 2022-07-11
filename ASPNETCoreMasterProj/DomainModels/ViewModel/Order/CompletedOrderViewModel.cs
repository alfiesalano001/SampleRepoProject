using System;
using System.Collections.Generic;
using DomainModels.ViewModel.OrderItem;
using DomainModels.Constants;
using DomainModels.Extensions;

namespace DomainModels.ViewModel.Order
{
    public class CompletedOrderViewModel : PlacedOrderViewModel
    {
        /// <summary>
        /// Returns a completed status
        /// </summary>
        public override OrderStatus Status { get => OrderStatus.Complete; }
    }
}
