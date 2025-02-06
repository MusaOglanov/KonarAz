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


		public async Task<IActionResult> Index(string categoryIds, string brandIds,string tagId,int page=1)
		{
			// Parse categoryIds and brandIds into arrays
			var categoryIdList = !string.IsNullOrEmpty(categoryIds)
				? categoryIds.Split(',').Select(id => int.Parse(id)).ToArray()
				: Array.Empty<int>();

			var brandIdList = !string.IsNullOrEmpty(brandIds)
				? brandIds.Split(',').Select(id => int.Parse(id)).ToArray()
				: Array.Empty<int>();

			// Base query
			var productsQuery = _db.Products
				.Include(p => p.ProductImages)
				.Include(p => p.Brand)
				.Include(p => p.ProductCategories)
				.ThenInclude(pc => pc.Category)
				.Include(p => p.ProductTags)
				.ThenInclude(pc => pc.Tag)
				.AsQueryable();

			// Apply filters
			if (categoryIdList.Any())
			{
				productsQuery = productsQuery.Where(p => p.ProductCategories
					.Any(pc => categoryIdList.Contains(pc.CategoryId)));
			}

			if (brandIdList.Any())
			{
				productsQuery = productsQuery.Where(p => brandIdList.Contains(p.BrandId));
			}

			if (!string.IsNullOrEmpty(tagId))
			{
				int tagIdParsed = int.Parse(tagId);
				productsQuery = productsQuery.Where(p => p.ProductTags.Any(t => t.Id == tagIdParsed));
			}

			int showCount = 2;

			ViewBag.PageCount = Math.Ceiling((decimal)await _db.Products.CountAsync() / showCount);
			ViewBag.CurrentPage = page;
			// Execute the query
			var products = await productsQuery.OrderByDescending(x=>x.Id).Skip((page - 1 )*showCount).Take(showCount).ToListAsync();

			// Set ViewBag for filters
			ViewBag.BackPhoto=await _db.BackPhotos.FirstOrDefaultAsync();
			ViewBag.Tags = await _db.Tags.ToListAsync();
			ViewBag.Categories = await _db.Categories.ToListAsync();
			ViewBag.Brands = await _db.Brands.ToListAsync();
			ViewBag.Products = await _db.Products
	             .Include(p => p.ProductCategories)
	             .Include(p => p.Brand)
	             .ToListAsync();
		

			return View(products);
		}






		
		public async Task<IActionResult> Detail(int? id)
		{
            ViewBag.BackPhoto = await _db.BackPhotos.FirstOrDefaultAsync();

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


        public async Task<IActionResult> ProductSearch(string key)
        {
            List<Product> products = await _db.Products.Include(x=>x.ProductImages).Where(x => x.Name.ToLower().Contains(key.ToLower())).ToListAsync();


            return PartialView("_ProductSearchPartialView", products);

        }

    }
}
