using DomainModels.Enum;
using DomainModels.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;

namespace ASPNETCoreMasterProj.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ApiBaseController<OrderController>
    {
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
            : base(logger) => _orderService = orderService.MustBeImplemented();

        /// <summary>
        /// Get order details by Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet(nameof(OrderController.Get))]
        public IActionResult Get(int orderId)
        {
            _logger.LogInformation("Controller OrderController -> Get");

            var order = _orderService.GetById(orderId);

            return Ok(order);
        }

        /// <summary>
        /// Get all Order Items by Id
        /// </summary>
        /// <param name="tableNumber"></param>
        /// <returns></returns>
        [HttpGet(nameof(OrderController.GetByTableNumber))]
        public IActionResult GetByTableNumber(int tableNumber)
        {
            _logger.LogInformation("Controller OrderController -> GetAllOrders");

            var items = _orderService.GetByTableNumber(tableNumber);

            return Ok(items);
        }

        /// <summary>
        /// Place an order for a customer or table
        /// </summary>
        /// <param name="tableNumber"></param>
        /// <returns></returns>
        [HttpPost(nameof(OrderController.Place))]
        public IActionResult Place(int tableNumber)
        {
            _logger.LogInformation($"Controller MenuController -> CreateOrder: {tableNumber}");

            var result = _orderService.PlaceOrder(tableNumber);

            return Ok(result);
        }

        /// <summary>
        /// Complete the order
        /// </summary>
        /// <param name="tableNumber"></param>
        /// <returns></returns>
        [HttpPost(nameof(OrderController.Complete))]
        public IActionResult Complete(int tableNumber)
        {
            var items = _orderService.Complete(tableNumber);

            return Ok(items);
        }

        /// <summary>
        /// Add recommended item based on category
        /// </summary>
        /// <param name="tableNumber"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPut(nameof(OrderController.AddChefRecommended))]
        public IActionResult AddChefRecommended(int tableNumber, Category category)
        {
            var result = _orderService.AddChefRecommended(tableNumber, category);

            return Ok(result);
        }

        /// <summary>
        /// Add an item to an order
        /// </summary>
        /// <param name="tableNumber"></param>
        /// <param name="menuId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [HttpPut(nameof(OrderController.AddOrderItem))]
        public IActionResult AddOrderItem(int tableNumber, int menuId, int quantity)
        {
            _logger.LogInformation($"Controller OrderController -> UpdateAllOrders: {tableNumber} {menuId}");

            var result = _orderService.AddOrderItem(tableNumber, menuId, quantity);

            return Ok(result);
        }

        /// <summary>
        /// This API will delete/cancel order per item.
        /// </summary>
        /// <param name="orderItemId"></param>
        /// <returns></returns>
        [HttpDelete(nameof(OrderController.CancelOrderItem))]
        public IActionResult CancelOrderItem(int orderItemId)
        {
            _logger.LogInformation($"Controller OrderController -> DeleteOrderPerItem: {orderItemId}");

            _orderService.CancelOrderItem(orderItemId);

            return Ok(orderItemId);
        }

        /// <summary>
        /// Get Chef recommendations only regardless of category
        /// quantity default is 1
        /// </summary>
        /// <param name="tableNumber"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [HttpPut(nameof(OrderController.AddAllChefRecommendation))]
        public IActionResult AddAllChefRecommendation(int tableNumber, int quantity = 1)
        {
            _logger.LogInformation("Controller OrderController -> GetChefsRecoByCategory");

            var items = _orderService.AddAllChefRecommendation(tableNumber, quantity);

            if (items == null) return new NotFoundResult();

            return Ok(items);
        }
    }
}
