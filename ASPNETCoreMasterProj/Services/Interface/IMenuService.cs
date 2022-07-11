using System.Collections.Generic;
using DomainModels.Enum;
using DomainModels.ViewModel.Menu;

namespace Services
{
    public interface IMenuService
    {
        MenuDetailsViewModel CreateMenuItem(MenuDetailsViewModel menu);
        IEnumerable<MenuDetailsViewModel> GetAllMenuItem();
        IEnumerable<MenuDetailsViewModel> GetMenuByCategory(Category category);
        IEnumerable<MenuDetailsViewModel> GetChefRecommendation(Category category);
    }
}
