namespace FakeStoreAPI.Host.DTOs.External
{
    public class FakeStoreProductDto
    {
        public long id { get; set; }
        public string? title { get; set; }
        public double price { get; set; }
        public string? description { get; set; }
        public string? category { get; set; }
        public string? image { get; set; }
        public RatingDto? rating { get; set; }

    }

    public class RatingDto
    {
        public double rate { get; set; }
        public int count { get; set; }
    }
}
