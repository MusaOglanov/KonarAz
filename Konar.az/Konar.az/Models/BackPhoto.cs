using System.ComponentModel.DataAnnotations.Schema;

namespace Konar.az.Models
{
	public class BackPhoto
	{
		public int Id { get; set; }
		public string BlogImage { get; set; }
		public string FaqImage { get; set; }
		public string ProductImage { get; set; }
		public string AccountImage { get; set; }

		[NotMapped]
		public IFormFile BlogPhoto { get; set; }
		[NotMapped]
		public IFormFile FaqPhoto { get; set; }
		[NotMapped]
		public IFormFile ProductPhoto { get; set; }
		[NotMapped]
		public IFormFile AccountPhoto { get; set; }


	}
}
