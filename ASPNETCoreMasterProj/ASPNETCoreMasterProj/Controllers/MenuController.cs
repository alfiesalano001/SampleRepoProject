using DomainModels.Enum;
using DomainModels.Extensions;
using DomainModels.ViewModel.Menu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;

namespace ASPNETCoreMasterProj.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MenuController : ApiBaseController<MenuController>
    {
        private readonly IMenuService menuService;

        public MenuController(ILogger<MenuController> logger, IMenuService menuService)
            : base(logger) => this.menuService = menuService.MustBeImplemented();

        /// <summary>
        /// Create a new menu item
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost(nameof(MenuController.Create))]
        public IActionResult Create([FromBody] MenuDetailsViewModel model)
        {
            _logger.LogInformation($"Controller MenuController -> CreateItem: {model}");

            var result = menuService.CreateMenuItem(model);

            return Ok(result);
        }

        /// <summary>
        /// Get all menu items
        /// </summary>
        /// <returns></returns>
        [HttpGet(nameof(MenuController.GetAll))]
        public IActionResult GetAll()
        {
            _logger.LogInformation("Controller MenuController -> GetAll");

            var items = menuService.GetAllMenuItem();

            return Ok(items);
        }

        /// <summary>
        /// Get menu items by category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpGet(nameof(MenuController.GetByCategory))]
        public IActionResult GetByCategory(Category category)
        {
            _logger.LogInformation("Controller MenuController -> GetMenuByCategory");

            var items = menuService.GetMenuByCategory(category);

            return Ok(items);
        }

        /// <summary>
        /// Get Chef recommendations by category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpGet(nameof(MenuController.GetChefRecommendation))]
        public IActionResult GetChefRecommendation(Category category)
        {
            _logger.LogInformation("Controller MenuController -> GetChefsRecoByCategory");

            var items = menuService.GetChefRecommendation(category);

            return Ok(items);
        }
    }
}
