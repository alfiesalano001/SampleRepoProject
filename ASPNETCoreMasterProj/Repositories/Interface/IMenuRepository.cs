using System.Linq;
using DomainModels.Entity;
using DomainModels.Enum;
using Repositories.Interface;

namespace Repositories
{
    public interface IMenuRepository : IGenericRepository<Menu>
    {
        IQueryable<Menu> GetMenuByCategory(Category category);
        Menu GetChefRecommendedByCategory(Category category);
        IQueryable<Menu> GetAllChefRecommended();
        IQueryable<Stock> GetAllStocks();
    }
}
