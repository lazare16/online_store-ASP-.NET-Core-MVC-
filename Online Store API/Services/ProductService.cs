using Online_Store_API.Models;

namespace Online_Store_API.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
    }

    public class ProductService : IProductService
    {
        private readonly List<Product> _products;

        public ProductService()
        {
            _products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Wireless Headphones",
                    Description = "High-quality wireless headphones with noise cancellation and 30-hour battery life. Perfect for music lovers and professionals.",
                    Price = 149.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=500&h=500&fit=crop",
                    Category = "Electronics",
                    Stock = 50
                },
                new Product
                {
                    Id = 2,
                    Name = "Smartphone",
                    Description = "Latest generation smartphone with 6.5-inch OLED display, 128GB storage, and advanced camera system.",
                    Price = 799.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?w=500&h=500&fit=crop",
                    Category = "Electronics",
                    Stock = 30
                },
                new Product
                {
                    Id = 3,
                    Name = "Laptop",
                    Description = "Powerful laptop with Intel i7 processor, 16GB RAM, 512GB SSD. Ideal for work and gaming.",
                    Price = 1299.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1496181133206-80ce9b88a853?w=500&h=500&fit=crop",
                    Category = "Electronics",
                    Stock = 20
                },
                new Product
                {
                    Id = 4,
                    Name = "Running Shoes",
                    Description = "Comfortable running shoes with cushioned sole and breathable material. Available in multiple colors.",
                    Price = 89.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=500&h=500&fit=crop",
                    Category = "Sports",
                    Stock = 100
                },
                new Product
                {
                    Id = 5,
                    Name = "Smart Watch",
                    Description = "Feature-rich smartwatch with fitness tracking, heart rate monitor, and smartphone notifications.",
                    Price = 249.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=500&h=500&fit=crop",
                    Category = "Electronics",
                    Stock = 45
                },
                new Product
                {
                    Id = 6,
                    Name = "Coffee Maker",
                    Description = "Programmable coffee maker with 12-cup capacity and auto shut-off feature. Start your day right!",
                    Price = 79.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1517668808822-9ebb02f2a0e6?w=500&h=500&fit=crop",
                    Category = "Home & Kitchen",
                    Stock = 60
                },
                new Product
                {
                    Id = 7,
                    Name = "Backpack",
                    Description = "Durable and spacious backpack with multiple compartments. Perfect for travel, school, or work.",
                    Price = 49.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=500&h=500&fit=crop",
                    Category = "Accessories",
                    Stock = 75
                },
                new Product
                {
                    Id = 8,
                    Name = "Gaming Mouse",
                    Description = "Ergonomic gaming mouse with customizable RGB lighting and 6 programmable buttons.",
                    Price = 59.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1527864550417-7fd91fc51a46?w=500&h=500&fit=crop",
                    Category = "Electronics",
                    Stock = 80
                }
            };
        }

        public Task<List<Product>> GetAllProductsAsync()
        {
            return Task.FromResult(_products);
        }

        public Task<Product?> GetProductByIdAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(product);
        }
    }
}
