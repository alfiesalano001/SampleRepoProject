using DomainModels.ViewModel.Menu;
using DomainModels.Constants;
using DomainModels.Extensions;

namespace DomainModels.ViewModel.OrderItem
{
    public class CompletedOrderItemViewModel : BaseOrderItemViewModel
    {
        /// <summary>
        /// Returns a pending status
        /// </summary>
        public override OrderStatus Status => OrderStatus.Complete;
    }
}
