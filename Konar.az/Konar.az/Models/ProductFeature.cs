namespace Konar.az.Models
{
	public class ProductFeature
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public Product Product { get; set; }
        public int? ProductId { get; set; }

    }
}
