using DomainModels.Constants;
using DomainModels.Extensions;
using DomainModels.ViewModel.Menu;

namespace DomainModels.ViewModel.OrderItem
{
    public abstract class BaseOrderItemViewModel
    {
        private int id;
        public int Id
        {
            get => this.id;
            set => this.id = value.MustBeGreaterThanZero(ErrorMessages.OrderItemIdInvalid);
        }

        private int quantity;
        public int Quantity
        {
            get => this.quantity;
            set => this.quantity = value.MustBeGreaterThanZero(ErrorMessages.OrderItemQuantityInvalidError);
        }

        private MenuDetailsViewModel menu;
        /// <summary>
        /// Returns the menu information
        /// </summary>
        public MenuDetailsViewModel Menu
        {
            get => this.menu;
            set => this.menu = value.MustNotBeNull(ErrorMessages.OrderItemMenuInvalid);
        }

        /// <summary>
        /// Returns the current status of the item
        /// </summary>
        public abstract OrderStatus Status { get; }

        /// <summary>
        /// Returns the base price multiplied by quantity
        /// </summary>
        public decimal TotalPrice { get => Menu.ItemPrice * Quantity; }
    }
}
