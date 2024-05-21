using System.ComponentModel.DataAnnotations.Schema;

namespace Konar.az.Models
{
    public class About
    {
        public int Id { get; set; }
        public string? WelcomeText { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Video { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        [NotMapped]
        public IFormFile Mp4 { get; set; }
        public bool IsDeActive { get; set; }
    }
}
