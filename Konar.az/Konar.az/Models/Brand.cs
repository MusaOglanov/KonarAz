using System.ComponentModel.DataAnnotations.Schema;

namespace Konar.az.Models
{
	public class Brand
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Image { get; set; }
		[NotMapped]
		public IFormFile Photo { get; set; }
		public List<Product> Products { get; set; }
		public bool IsDeactive { get; set; }

	}
}
