namespace School.Cafeteria.Models.Models.Product
{
    public class ProductDto
    {
        public int? ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        public string Image { get; set; }
        public bool IsAvailable { get; set; }
    }
}
