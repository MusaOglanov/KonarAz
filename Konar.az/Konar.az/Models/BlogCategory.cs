namespace Konar.az.Models
{
	public class BlogCategory
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Blog> Blogs { get; set; }
        public bool IsDeActive { get; set; }
    }
}
