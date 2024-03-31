using System.ComponentModel.DataAnnotations.Schema;

namespace Konar.az.Models
{
	public class ProductFeature
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public Product Product { get; set; }
        [ForeignKey("Product")]
        public int? ProductId { get; set; }

    }
}
