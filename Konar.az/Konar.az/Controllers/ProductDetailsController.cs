using Konar.az.DAL;
using Konar.az.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Konar.az.Controllers
{
	public class ProductDetailsController : Controller
	{
		private readonly AppDbContext _db;
		public ProductDetailsController(AppDbContext db)
		{
			_db = db;
		}
		public async Task<IActionResult> Index()
		{
			List<Product> product = await _db.Products
				.Include(x => x.ProductDetail)
				.Include(x => x.ProductImages)
				.Include(x => x.ProductFeatures)
				.Include(x => x.Brand)
				.Include(x => x.ProductCategories)
				.ThenInclude(x => x.Category)
				.Include(x => x.ProductTags)
				.ThenInclude(x => x.Tag)
				.Include(x => x.ProductDetail)
				.ToListAsync();
			return View(product);
		}
	}
}
