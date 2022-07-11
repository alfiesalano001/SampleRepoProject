using System.Linq;
using DomainModels.Constants;
using DomainModels.Entity;
using DomainModels.Enum;
using DomainModels.Extensions;
using Repositories.Interface;

namespace Repositories
{
    public class MenuRepository : GenericRepository<Menu>, IMenuRepository
    {
        public MenuRepository(IDataContext dbContext) : base(dbContext) { }

        public Menu GetChefRecommendedByCategory(Category category) => GetAll().FirstOrDefault(a => a.Category == category && a.ChefRecommendation);

        public IQueryable<Menu> GetMenuByCategory(Category category) => GetAll().Where(a => a.Category == category);

        public IQueryable<Menu> GetAllChefRecommended() => GetAll().Where(a => a.ChefRecommendation);

        public IQueryable<Stock> GetAllStocks() => base.dbContext.GetAll<Stock>();
    }
}
