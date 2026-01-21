using Microsoft.AspNetCore.Mvc;
using online_store_MVC.Models;
using System.Text;
using System.Text.Json;

namespace online_store_MVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<OrderController> _logger;
        private const string UserIdKey = "UserId";

        public OrderController(IHttpClientFactory httpClientFactory, ILogger<OrderController> logger)
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

        public async Task<IActionResult> Checkout()
        {
            var userId = GetUserId();
            Cart? cart = null;

            try
            {
                var client = _httpClientFactory.CreateClient("OnlineStoreAPI");
                var response = await client.GetAsync($"api/Cart/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    cart = await response.Content.ReadFromJsonAsync<Cart>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching cart: {ex.Message}");
            }

            if (cart == null || !cart.Items.Any())
            {
                TempData["ErrorMessage"] = "Your cart is empty. Please add items before checkout.";
                return RedirectToAction("Index", "Cart");
            }

            ViewBag.Cart = cart;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(Order orderModel)
        {
            var userId = GetUserId();

            try
            {
                // Get cart items
                var client = _httpClientFactory.CreateClient("OnlineStoreAPI");
                var cartResponse = await client.GetAsync($"api/Cart/{userId}");

                if (!cartResponse.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Failed to retrieve cart information.";
                    return RedirectToAction("Checkout");
                }

                var cart = await cartResponse.Content.ReadFromJsonAsync<Cart>();

                if (cart == null || !cart.Items.Any())
                {
                    TempData["ErrorMessage"] = "Your cart is empty.";
                    return RedirectToAction("Index", "Cart");
                }

                // Create order object
                var order = new Order
                {
                    UserId = userId,
                    CustomerName = orderModel.CustomerName,
                    Email = orderModel.Email,
                    PhoneNumber = orderModel.PhoneNumber,
                    Address = orderModel.Address,
                    City = orderModel.City,
                    State = orderModel.State,
                    ZipCode = orderModel.ZipCode,
                    TotalAmount = cart.TotalAmount,
                    OrderItems = cart.Items.Select(item => new OrderItem
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Price = item.Price,
                        Quantity = item.Quantity
                    }).ToList()
                };

                // Send order to API
                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(order),
                    Encoding.UTF8,
                    "application/json"
                );

                var orderResponse = await client.PostAsync("api/Order", jsonContent);

                if (orderResponse.IsSuccessStatusCode)
                {
                    var createdOrder = await orderResponse.Content.ReadFromJsonAsync<Order>();

                    // Clear the cart after successful order
                    await client.DeleteAsync($"api/Cart/clear/{userId}");

                    TempData["SuccessMessage"] = "Order placed successfully!";
                    return RedirectToAction("Confirmation", new { orderNumber = createdOrder?.OrderNumber });
                }
                else
                {
                    var errorContent = await orderResponse.Content.ReadAsStringAsync();
                    _logger.LogError($"Failed to place order. Status: {orderResponse.StatusCode}, Error: {errorContent}");
                    TempData["ErrorMessage"] = "Failed to place order. Please try again.";
                    return RedirectToAction("Checkout");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error placing order: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while placing your order.";
                return RedirectToAction("Checkout");
            }
        }

        public async Task<IActionResult> Confirmation(string orderNumber)
        {
            if (string.IsNullOrEmpty(orderNumber))
            {
                return RedirectToAction("Index", "Home");
            }

            Order? order = null;

            try
            {
                var client = _httpClientFactory.CreateClient("OnlineStoreAPI");
                var response = await client.GetAsync($"api/Order/number/{orderNumber}");

                if (response.IsSuccessStatusCode)
                {
                    order = await response.Content.ReadFromJsonAsync<Order>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching order: {ex.Message}");
            }

            if (order == null)
            {
                TempData["ErrorMessage"] = "Order not found.";
                return RedirectToAction("Index", "Home");
            }

            return View(order);
        }

        public async Task<IActionResult> MyOrders()
        {
            var userId = GetUserId();
            List<Order> orders = new List<Order>();

            try
            {
                var client = _httpClientFactory.CreateClient("OnlineStoreAPI");
                var response = await client.GetAsync($"api/Order/user/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    orders = await response.Content.ReadFromJsonAsync<List<Order>>() ?? new List<Order>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching orders: {ex.Message}");
            }

            return View(orders);
        }
    }
}
