using Online_Store_API.Models;

namespace Online_Store_API.Services
{
    public interface ICartService
    {
        Task<Cart> GetCartAsync(string userId);
        Task<Cart> AddToCartAsync(string userId, int productId, int quantity);
        Task<Cart> UpdateCartItemAsync(string userId, int productId, int quantity);
        Task<Cart> RemoveFromCartAsync(string userId, int productId);
        Task ClearCartAsync(string userId);
    }

    public class CartService : ICartService
    {
        private readonly IProductService _productService;
        private static readonly Dictionary<string, Cart> _carts = new Dictionary<string, Cart>();

        public CartService(IProductService productService)
        {
            _productService = productService;
        }

        public Task<Cart> GetCartAsync(string userId)
        {
            if (!_carts.ContainsKey(userId))
            {
                _carts[userId] = new Cart { UserId = userId };
            }
            return Task.FromResult(_carts[userId]);
        }

        public async Task<Cart> AddToCartAsync(string userId, int productId, int quantity)
        {
            var cart = await GetCartAsync(userId);
            var product = await _productService.GetProductByIdAsync(productId);

            if (product == null)
            {
                throw new ArgumentException($"Product with ID {productId} not found.");
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity,
                    ImageUrl = product.ImageUrl
                });
            }

            return cart;
        }

        public async Task<Cart> UpdateCartItemAsync(string userId, int productId, int quantity)
        {
            var cart = await GetCartAsync(userId);
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                if (quantity > 0)
                {
                    item.Quantity = quantity;
                }
                else
                {
                    cart.Items.Remove(item);
                }
            }

            return cart;
        }

        public async Task<Cart> RemoveFromCartAsync(string userId, int productId)
        {
            var cart = await GetCartAsync(userId);
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                cart.Items.Remove(item);
            }

            return cart;
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await GetCartAsync(userId);
            cart.Items.Clear();
        }
    }
}
