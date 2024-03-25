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
	}
	
}
