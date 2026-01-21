using Microsoft.AspNetCore.Mvc;
using Online_Store_API.Models;
using Online_Store_API.Services;

namespace Online_Store_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // POST: api/Order
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdOrder = await _orderService.CreateOrderAsync(order);
                return CreatedAtAction(nameof(GetOrderByOrderNumber), new { orderNumber = createdOrder.OrderNumber }, createdOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the order.", error = ex.Message });
            }
        }

        // GET: api/Order/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound(new { message = "Order not found." });
            }

            return Ok(order);
        }

        // GET: api/Order/number/{orderNumber}
        [HttpGet("number/{orderNumber}")]
        public async Task<ActionResult<Order>> GetOrderByOrderNumber(string orderNumber)
        {
            var order = await _orderService.GetOrderByOrderNumberAsync(orderNumber);

            if (order == null)
            {
                return NotFound(new { message = "Order not found." });
            }

            return Ok(order);
        }

        // GET: api/Order/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<Order>>> GetUserOrders(string userId)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }
    }
}
