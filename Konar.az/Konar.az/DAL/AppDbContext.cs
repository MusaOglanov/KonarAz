using Konar.az.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Konar.az.DAL
{
	public class AppDbContext: IdentityDbContext<AppUser>
	{
        public AppDbContext(DbContextOptions<AppDbContext> option):base(option)
        {
                
        }

		

		public DbSet<Faq> Faqs { get; set; }
		public DbSet<Blog> Blogs { get; set; }
		public DbSet<BlogCategory> BlogCategories { get; set; }
		public DbSet<BlogTag> BlogTags { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<About> Abouts { get; set; }
		public DbSet<Statistics> Statistics { get; set; }
		public DbSet<Brand> Brands { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductDetail> ProductDetails { get; set; }
		public DbSet<ProductImage> ProductImages { get; set; }
		public DbSet<ProductTag> ProductTags { get; set; }
		public DbSet<ProductCategory> ProductCategories { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<ProductFeature> ProductFeatures { get; set; }
		public DbSet<Position> Positions { get; set; }
		public DbSet<Employee> Employee { get; set; }
		public DbSet<Slider> Sliders { get; set; }
		public DbSet<Bio> Bios { get; set; }
		public DbSet<ContactMessage> ContactMessages { get; set; }
		public DbSet<Contact> Contacts { get; set; }
		public DbSet<CaseStudy> CaseStudies { get; set; }
		public DbSet<BackPhoto> BackPhotos { get; set; }
		public DbSet<HomeVideo> HomeVideos { get; set; }
	}

}
