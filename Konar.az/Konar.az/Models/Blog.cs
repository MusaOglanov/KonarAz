using System.ComponentModel.DataAnnotations.Schema;

namespace Konar.az.Models
{
	public class Blog
	{
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public DateTime CreateTime { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public bool IsDeActive { get; set; }
        public BlogCategory BlogCategory { get; set; }
        public int BlogCategoryId { get; set; }
        public List<BlogTag> BlogTags { get; set; }

    }
}
