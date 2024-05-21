namespace Konar.az.Models
{
	public class Brand
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<Product> Products { get; set; }
		public bool IsDeactive { get; set; }

	}
}
