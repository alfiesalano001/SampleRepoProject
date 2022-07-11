using System;
using DomainModels.Constants;
using DomainModels.Extensions;
using DomainModels.ViewModel.Menu;

namespace DomainModels.ViewModel.OrderItem
{
    public class PlacedOrderItemViewModel : BaseOrderItemViewModel
    {
        private DateTime datePlaced;
        /// <summary>
        /// Returns the date when the item has been placed
        /// </summary>
        public DateTime DatePlaced
        {
            get => this.datePlaced;
            set => this.datePlaced = value.MustBeLessThan(DateTime.UtcNow, ErrorMessages.OrderItemDateInvalid);
        }

        /// <summary>
        /// Returns a calculated value of the status depending on the remaining time and cooking time
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
        /// Returns the total cooking time of the item
        /// </summary>
        public int TotalCookTimeInSec { get => base.Menu.CookTimeInSec * Quantity; }

        /// <summary>
        /// Returns the total preparation time of the item
        /// </summary>
        public int TotalPrepTimeInSec { get => base.Menu.PrepTimeInSec * Quantity; }

        /// <summary>
        /// Returns the remaining preparation time of the item
        /// </summary>
        public int RemainingPreparationTime
        {
            get
            {
                var estimatedCooked = this.DatePlaced.AddSeconds(this.TotalPrepTimeInSec);
                var remaining = (int)(estimatedCooked - DateTime.UtcNow).TotalSeconds;

                return remaining < 0 ? 0 : remaining;
            }
        }

        /// <summary>
        /// Returns the remaining cooking time of the item
        /// </summary>
        public int RemainingCookingTime
        {
            get
            {
                if (this.RemainingPreparationTime > 0) return this.TotalCookTimeInSec;

                var estimatedPrepared = this.DatePlaced.AddSeconds(this.TotalPrepTimeInSec + this.TotalCookTimeInSec);
                var remaining = (int)(estimatedPrepared - DateTime.UtcNow).TotalSeconds;

                return remaining < 0 ? 0 : remaining;
            }
        }
    }
}
