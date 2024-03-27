using Konar.az.Models;
using Microsoft.EntityFrameworkCore;

namespace Konar.az.DAL
{
	public class AppDbContext: DbContext
	{
        public AppDbContext(DbContextOptions<AppDbContext> option):base(option)
        {
                
        }
		public DbSet<Test> Test { get; set; }
		public DbSet<Faq> Faqs { get; set; }
		public DbSet<Blog> Blogs { get; set; }
		public DbSet<BlogCategory> BlogCategories { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<BlogTag> BlogTags { get; set; }
		public DbSet<About> Abouts { get; set; }
		public DbSet<Statistics> Statistics { get; set; }
	}
	
}
