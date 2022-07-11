using DomainModels.Constants;
using DomainModels.Enum;
using DomainModels.Extensions;

namespace DomainModels.ViewModel.Menu
{
    public class MenuDetailsViewModel
    {
        private int id;
        public int Id
        {
            get => this.id;
            set => this.id = value.MustBeGreaterThanZero(ErrorMessages.MenuIdInvalidError);
        }

        private string name;
        public string Name
        {
            get => this.name;
            set => this.name = value.MustNotBeEmpty(ErrorMessages.MenuNameInvalid);
        }

        private string description;
        public string Description
        {
            get => this.description;
            set => this.description = value.MustNotBeEmpty(ErrorMessages.MenuDescriptionInvalid);
        }

        private Category category;
        public Category Category
        {
            get => this.category;
            set => this.category = value.MustBeValid(ErrorMessages.MenuCategoryInvalid);
        }

        private decimal itemPrice;
        public decimal ItemPrice
        {
            get => this.itemPrice;
            set => this.itemPrice = value.MustBeGreaterThanZero(ErrorMessages.MenuPriceInvalid);
        }

        private int prepTimeInSec;
        public int PrepTimeInSec
        {
            get => this.prepTimeInSec;
            set => this.prepTimeInSec = value.MustBePositive(ErrorMessages.MenuPreparationTimeInvalid);
        }

        private int cookTimeInSec;
        public int CookTimeInSec
        {
            get => this.cookTimeInSec;
            set => this.cookTimeInSec = value.MustBePositive(ErrorMessages.MenuCookingTimeInvalid);
        }

        public bool ChefRecommendation { get; set; }
    }
}
