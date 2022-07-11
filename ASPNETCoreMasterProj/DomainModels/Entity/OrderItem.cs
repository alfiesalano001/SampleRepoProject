using System;

namespace DomainModels.Entity
{
    public class OrderItem : EntityBase
    {
        public int MenuId { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public bool IsPlaced { get; set; }
        public DateTime? DatePlaced { get; set; }
    }
}
