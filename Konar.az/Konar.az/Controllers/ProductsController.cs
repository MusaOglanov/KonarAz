using Konar.az.DAL;
using Konar.az.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Konar.az.Controllers
{
	public class ProductsController : Controller
	{
		private readonly AppDbContext _db;
        public ProductsController(AppDbContext db)
        {
			_db = db;
        }
        public async Task<IActionResult> Index()
		{
			List<Product> products = await _db.Products
				.Include(x => x.ProductDetail)
				.Include(x => x.ProductImages)
				.Include(x => x.Brand)
				.Include(x => x.ProductCategories)
				.ThenInclude(x => x.Category)
				.Include(x => x.ProductTags)
				.ThenInclude(x => x.Tag)
				.Include(x => x.ProductDetail)
				.ToListAsync();
			return View(products);
		}

		public async Task<IActionResult> Detail(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			Product? product = await _db.Products
				.Include(x => x.ProductDetail)
				.Include(x => x.ProductFeatures)
				.Include(x => x.ProductImages)
				.Include(x => x.Brand)
				.Include(x => x.ProductCategories)
				.ThenInclude(x => x.Category)
				.Include(x => x.ProductTags)
				.ThenInclude(x => x.Tag)
				.FirstOrDefaultAsync(t => t.Id == id);
			if (product == null)
			{
				return BadRequest();
			}
			return View(product);
		}

	}
}
