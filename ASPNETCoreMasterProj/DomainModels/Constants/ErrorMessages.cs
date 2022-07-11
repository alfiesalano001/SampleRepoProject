namespace DomainModels.Constants
{
    public static class ErrorMessages
    {
        //Menu Encapsulation Error Messages
        public const string MenuIdInvalidError = "Menu Id must be greater than zero.";
        public const string MenuNameInvalid = "Menu Name must not be empty.";
        public const string MenuDescriptionInvalid = "Menu Description must not be empty.";
        public const string MenuCategoryInvalid = "Menu Category is not valid.";
        public const string MenuPriceInvalid = "Menu Price must be greater than zero.";
        public const string MenuPreparationTimeInvalid = "Menu Preparation Time must be a positive number or zero.";
        public const string MenuCookingTimeInvalid = "Menu Cooking Time must be a positive number or zero.";

        public const string OrderIdInvalid = "Order Id must be greater than zero.";
        public const string OrderTableNumberInvalid = "Order table number must be greater than zero.";
        public const string OrderDateInvalid = "Order date must be less than today.";

        public const string OrderItemsInvalid = "Order must have at least one order item.";
        public const string OrderItemIdInvalid = "Order item Id must be greater than zero.";
        public const string OrderItemQuantityInvalidError = "Order item Quantity must be greater than zero.";
        public const string OrderItemDateInvalid = "Order item date placed must be less than today.";
        public const string OrderItemMenuInvalid = "Order item menu is missing.";

        public static class NotFound
        {
            //Not Found Error Messages
            public const string AllMenu = "Could not find any items in the menu.";
            public const string AllMenuCategory = "Could not find any items in the category.";
            public const string MenuError = "Could not retrieve menu details.";
            public const string OrderError = "Could not retrieve order details.";
            public const string OrderItemError = "Could not retrieve order item details.";
            public const string OrderItemMenuError = "Could not retrieve menu details for an order item.";
            public const string PendingOrderItemMenuError = "Could not retrieve menu details for pending order item.";
            public const string PlacedOrderItemMenuError = "Could not retrieve menu details for placed order item.";
            public const string CompletedOrderItemMenuError = "Could not retrieve menu details for pending order item.";
            public const string AllChefRecommendationsError = "Could not find any chef recommendations.";
            public const string ChefRecommendedCategoryError = "Could not find any chef's recommended menu for the specified category.";
        }

        public static class Operations
        {
            //Add Order Error Messages
            public const string AddOrderMenuIdInvalidError = "Could not add an order as the menu id must be greater than zero.";
            public const string AddOrderQuantityInvalidError = "Could not add an order as the quantity must be greater than or equal to zero";

            //Complete Order Error Messages
            public const string CompleteOrderInvalid = "Could not complete the order as there are no items placed.";
            public const string CompleteOrderPendingOrdersError = "Could not complete the order as there are pending orders.";
            public const string CompleteOrderNotServedError = "Could not complete the order as there are orders that have not been served.";

            //Cancel Order Error Messages
            public const string CancelOrderItemIdInvalid = "Could not cancel the order item as the order item id must be greater than or equal to zero.";
            public const string CancelPlacedOrderError = "Could not cancel the order item as it has already been placed.";

            //Place Order Error Messages
            public const string PlaceOrderTableNumberInvalidError = "Could not place an order as the table number must be greater than zero.";
            public const string PlaceOrderEmptyPendingItemsError = "Could not place order as there are no items to place.";
        }
    }
}
