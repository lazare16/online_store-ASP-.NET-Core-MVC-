namespace Online_Store_API.Models
{
    public class Cart
    {
        public string UserId { get; set; } = string.Empty;
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal TotalAmount => Items.Sum(item => item.Price * item.Quantity);
    }
}
