using System.Collections.Generic;
using DomainModels.Enum;

namespace DomainModels.Entity
{
    public class Menu : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public decimal ItemPrice { get; set; }
        public int PrepTimeInSec { get; set; }
        public int CookTimeInSec { get; set; }
        public bool ChefRecommendation { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
    }
}
