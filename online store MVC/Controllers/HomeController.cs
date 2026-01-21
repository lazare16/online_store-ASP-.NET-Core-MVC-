using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using online_store_MVC.Models;

namespace online_store_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IHttpClientFactory httpClientFactory, ILogger<HomeController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var products = new List<Product>();

            try
            {
                var client = _httpClientFactory.CreateClient("OnlineStoreAPI");
                var response = await client.GetAsync("api/Products");

                if (response.IsSuccessStatusCode)
                {
                    products = await response.Content.ReadFromJsonAsync<List<Product>>() ?? new List<Product>();
                }
                else
                {
                    _logger.LogError($"API call failed with status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling API: {ex.Message}");
            }

            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
