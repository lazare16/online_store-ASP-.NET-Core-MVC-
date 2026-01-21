using Microsoft.AspNetCore.Mvc;
using online_store_MVC.Models;

namespace online_store_MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IHttpClientFactory httpClientFactory, ILogger<ProductsController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<IActionResult> Details(int id)
        {
            Product? product = null;

            try
            {
                var client = _httpClientFactory.CreateClient("OnlineStoreAPI");
                var response = await client.GetAsync($"api/Products/{id}");

                if (response.IsSuccessStatusCode)
                {
                    product = await response.Content.ReadFromJsonAsync<Product>();
                }
                else
                {
                    _logger.LogError($"API call failed with status code: {response.StatusCode}");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling API: {ex.Message}");
                return NotFound();
            }

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
