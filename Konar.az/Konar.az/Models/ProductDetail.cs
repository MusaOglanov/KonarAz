using System.ComponentModel.DataAnnotations.Schema;

namespace Konar.az.Models
{
	public class ProductDetail
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Material { get; set; }
		public string FeautersName { get; set; }
		public string FeautersValue { get; set; }
		public bool HasStock { get; set; }
		public Product Product { get; set; }
		[ForeignKey("Product")]
		public int ProductId { get; set; }
	}
}
