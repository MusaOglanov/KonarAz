using System.ComponentModel.DataAnnotations.Schema;

namespace Konar.az.Models
{
    public class HomeVideo
    {
        public int Id { get; set; }
        public string? SlideVideo { get; set; }
        [NotMapped]
        public IFormFile? Mp4 { get; set; }
    }
}
