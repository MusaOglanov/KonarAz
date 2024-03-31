using Konar.az.DAL;
using Konar.az.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Konar.az.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _db;

        public ProductsController(AppDbContext db)
        {
            _db = db;
        }
        #region Index
        public async Task<IActionResult> Index()
        {
            List<Product> product = await _db.Products
                 .Include(x => x.ProductDetail)
                 .Include(x => x.ProductFeatures)
                 .Include(x => x.ProductImages)
                 .Include(x => x.Brand)
                 .Include(x => x.ProductCategories)
                 .ThenInclude(x => x.Category)
                 .Include(x => x.ProductTags)
                 .ThenInclude(x => x.Tag)
                 .ToListAsync();
            return View(product);
        }
        #endregion


        #region Create

        #region Get
        public async Task<IActionResult> Create()
        {
            ViewBag.Brands = await _db.Brands.ToListAsync();
            ViewBag.Tags = await _db.Tags.ToListAsync();
            ViewBag.Categories = await _db.Categories.ToListAsync();
            ViewBag.Feauters = await _db.ProductFeatures.ToListAsync();
            return View();
        }
        #endregion
        
        #region Post

        #endregion

        #endregion


    }
}
