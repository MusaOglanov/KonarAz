using System.ComponentModel.DataAnnotations.Schema;

namespace Konar.az.Models
{
	public class Faq
	{
        public int Id { get; set; }
        public string? Question { get; set; }
        public string? Answer { get; set; }
        public bool IsDeactive { get; set; }
    }
}
