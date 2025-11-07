namespace FakeStoreAPI.Host.DTOs.External
{
    public class FakeStoreCartDto
    {
        public long id { get; set; }
        public long userId { get; set; }
        public string? date { get; set; }
        public Products[]? products { get; set; }
        public int __v { get; set; }
    }

    public class Products
    {
        public long productId { get; set; }
        public int quantity { get; set; }
    } 
}
