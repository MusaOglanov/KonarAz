using System.ComponentModel.DataAnnotations.Schema;

namespace Konar.az.Models
{
	public class Slider
	{
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public bool IsDeactive { get; set; }
    }
}
