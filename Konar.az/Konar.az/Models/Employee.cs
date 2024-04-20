using System.ComponentModel.DataAnnotations.Schema;

namespace Konar.az.Models
{
	public class Employee
	{
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
        public bool IsDeactive { get; set; }
        public Position Position { get; set; }
		public int PositionId { get; set; }
	}
}
