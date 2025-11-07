using FakeStoreAPI.Host.DTOs.External;

namespace FakeStoreAPI.Host.DTOs.Internal
{
    public class CartDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public DateTime? Date { get; set; }
        public Products[]? Products { get; set; }
    }
    public class Products
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
