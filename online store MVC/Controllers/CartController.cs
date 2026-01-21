using Microsoft.AspNetCore.Mvc;
using online_store_MVC.Models;
using System.Text;
using System.Text.Json;

namespace online_store_MVC.Controllers
{
    public class CartController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CartController> _logger;
        private const string UserIdKey = "UserId";

        public CartController(IHttpClientFactory httpClientFactory, ILogger<CartController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        private string GetUserId()
        {
            var userId = HttpContext.Session.GetString(UserIdKey);
            if (string.IsNullOrEmpty(userId))
            {
                userId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString(UserIdKey, userId);
            }
            return userId;
        }

        public async Task<IActionResult> Index()
        {
            Cart? cart = null;
            var userId = GetUserId();

            try
            {
                var client = _httpClientFactory.CreateClient("OnlineStoreAPI");
                var response = await client.GetAsync($"api/Cart/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    cart = await response.Content.ReadFromJsonAsync<Cart>();
                }
                else
                {
                    _logger.LogError($"API call failed with status code: {response.StatusCode}");
                    cart = new Cart { UserId = userId };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling API: {ex.Message}");
                cart = new Cart { UserId = userId };
            }

            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var userId = GetUserId();

            try
            {
                var client = _httpClientFactory.CreateClient("OnlineStoreAPI");
                
                var requestData = new
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity
                };

                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(requestData),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PostAsync("api/Cart/add", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Product added to cart successfully!";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Failed to add product to cart. Status: {response.StatusCode}, Error: {errorContent}");
                    TempData["ErrorMessage"] = "Failed to add product to cart.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding product to cart: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while adding the product to cart.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            var userId = GetUserId();

            try
            {
                var client = _httpClientFactory.CreateClient("OnlineStoreAPI");

                var requestData = new
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity
                };

                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(requestData),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PutAsync("api/Cart/update", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cart updated successfully!";
                }
                else
                {
                    _logger.LogError($"Failed to update cart. Status: {response.StatusCode}");
                    TempData["ErrorMessage"] = "Failed to update cart.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating cart: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while updating the cart.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userId = GetUserId();

            try
            {
                var client = _httpClientFactory.CreateClient("OnlineStoreAPI");

                var requestData = new
                {
                    UserId = userId,
                    ProductId = productId
                };

                var request = new HttpRequestMessage(HttpMethod.Delete, "api/Cart/remove")
                {
                    Content = new StringContent(
                        JsonSerializer.Serialize(requestData),
                        Encoding.UTF8,
                        "application/json"
                    )
                };

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Product removed from cart!";
                }
                else
                {
                    _logger.LogError($"Failed to remove product from cart. Status: {response.StatusCode}");
                    TempData["ErrorMessage"] = "Failed to remove product from cart.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error removing product from cart: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while removing the product.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetUserId();

            try
            {
                var client = _httpClientFactory.CreateClient("OnlineStoreAPI");
                var response = await client.DeleteAsync($"api/Cart/clear/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cart cleared successfully!";
                }
                else
                {
                    _logger.LogError($"Failed to clear cart. Status: {response.StatusCode}");
                    TempData["ErrorMessage"] = "Failed to clear cart.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error clearing cart: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while clearing the cart.";
            }

            return RedirectToAction("Index");
        }
    }
}
