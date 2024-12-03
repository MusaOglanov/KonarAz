using Konar.az.Models;

namespace Konar.az.ViewModels
{
    public class BlogVM
    {
        public List<Blog> Blogs { get; set; }
        public List<BlogCategory> BlogCategories { get; set; }
        public List<Tag> Tags { get; set; }
        public BackPhoto BackPhoto { get; set; }
    }
}
