using System;
using System.Collections.Generic;

namespace DomainModels.Entity
{
    public class Order : EntityBase
    {
        public Order()
        {
            this.OrderItems = new List<OrderItem>();
        }

        public int TableNumber { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime Date { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
