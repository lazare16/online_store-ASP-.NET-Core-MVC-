using Microsoft.AspNetCore.Mvc;
using Online_Store_API.Models;
using Online_Store_API.Services;

namespace Online_Store_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // GET: api/Cart/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<Cart>> GetCart(string userId)
        {
            var cart = await _cartService.GetCartAsync(userId);
            return Ok(cart);
        }

        // POST: api/Cart/add
        [HttpPost("add")]
        public async Task<ActionResult<Cart>> AddToCart([FromBody] AddToCartRequest request)
        {
            try
            {
                var cart = await _cartService.AddToCartAsync(request.UserId, request.ProductId, request.Quantity);
                return Ok(cart);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // PUT: api/Cart/update
        [HttpPut("update")]
        public async Task<ActionResult<Cart>> UpdateCartItem([FromBody] UpdateCartRequest request)
        {
            var cart = await _cartService.UpdateCartItemAsync(request.UserId, request.ProductId, request.Quantity);
            return Ok(cart);
        }

        // DELETE: api/Cart/remove
        [HttpDelete("remove")]
        public async Task<ActionResult<Cart>> RemoveFromCart([FromBody] RemoveFromCartRequest request)
        {
            var cart = await _cartService.RemoveFromCartAsync(request.UserId, request.ProductId);
            return Ok(cart);
        }

        // DELETE: api/Cart/clear/{userId}
        [HttpDelete("clear/{userId}")]
        public async Task<IActionResult> ClearCart(string userId)
        {
            await _cartService.ClearCartAsync(userId);
            return NoContent();
        }
    }

    public class AddToCartRequest
    {
        public string UserId { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }

    public class UpdateCartRequest
    {
        public string UserId { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class RemoveFromCartRequest
    {
        public string UserId { get; set; } = string.Empty;
        public int ProductId { get; set; }
    }
}
