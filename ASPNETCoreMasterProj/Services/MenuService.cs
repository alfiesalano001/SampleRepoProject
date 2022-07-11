using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DomainModels.Entity;
using DomainModels.Enum;
using DomainModels.Extensions;
using DomainModels.ViewModel.Menu;
using Repositories;
using Services.Extensions;

namespace Services
{
    public sealed class MenuService : IMenuService
    {
        private readonly IMenuRepository menuRepository;
        private readonly IMapper mapper;

        public MenuService(IMapper mapper, IMenuRepository repository)
        {
            this.menuRepository = repository.MustBeImplemented();
            this.mapper = mapper.MustBeImplemented();
        }

        public MenuDetailsViewModel CreateMenuItem(MenuDetailsViewModel menu)
        {
            var map = this.mapper.Map<Menu>(menu);

            menuRepository.Add(map);

            return map.CreateMenuDetails();
        }

        public IEnumerable<MenuDetailsViewModel> GetAllMenuItem()
        {
            var result = menuRepository.GetAll();

            return result.Select(_ => _.CreateMenuDetails());
        }

        public IEnumerable<MenuDetailsViewModel> GetMenuByCategory(Category category)
        {
            var result = this.menuRepository.GetMenuByCategory(category);

            return result.Select(_ => _.CreateMenuDetails());
        }

        public IEnumerable<MenuDetailsViewModel> GetChefRecommendation(Category category)
        {
            var result = this.menuRepository.GetMenuByCategory(category).Where(_ => _.ChefRecommendation);

            return result.Select(_ => _.CreateMenuDetails());
        }
    }
}
