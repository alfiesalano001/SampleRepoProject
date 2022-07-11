using System.Collections.Generic;
using DomainModels.Constants;
using DomainModels.Enum;
using DomainModels.Exceptions;
using DomainModels.Extensions;
using DomainModels.Helpers;
using DomainModels.ViewModel.Menu;
using Microsoft.Extensions.Logging;

namespace Services.Decorators
{
    public class MenuServiceDecorator : IMenuService
    {
        private readonly IMenuService menuService;
        private readonly ILogger<MenuServiceDecorator> logger;

        public MenuServiceDecorator(ILogger<MenuServiceDecorator> logger, IMenuService menuService)
        {
            this.menuService = menuService;
            this.logger = logger;
        }

        public MenuDetailsViewModel CreateMenuItem(MenuDetailsViewModel menu)
        {
            this.logger.LogInformation($"MenuService -> CreateMenuItem: {menu}");

            Throw<BadRequestException>.IfNull(menu, "Menu model is invalid.");

            return this.menuService.CreateMenuItem(menu).MustNotBeNull(ErrorMessages.NotFound.MenuError);
        }

        public IEnumerable<MenuDetailsViewModel> GetAllMenuItem()
        {
            this.logger.LogInformation("MenuService -> GetAllMenuItem");

            return this.menuService.GetAllMenuItem().MustNotBeNullOrEmpty(ErrorMessages.NotFound.AllMenu);
        }

        public IEnumerable<MenuDetailsViewModel> GetChefRecommendation(Category category)
        {
            this.logger.LogInformation("MenuService -> GetMenuByCategory");

            Throw<BadRequestException>.IfNotDefined(category, ErrorMessages.MenuCategoryInvalid);

            return this.menuService.GetChefRecommendation(category).MustNotBeNullOrEmpty(ErrorMessages.NotFound.ChefRecommendedCategoryError);
        }

        public IEnumerable<MenuDetailsViewModel> GetMenuByCategory(Category category)
        {
            this.logger.LogInformation("MenuService -> GetMenuByCategory");

            Throw<BadRequestException>.IfNotDefined(category, ErrorMessages.MenuCategoryInvalid);

            return this.menuService.GetMenuByCategory(category).MustNotBeNullOrEmpty(ErrorMessages.NotFound.AllMenuCategory);
        }
    }
}
