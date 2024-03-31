using Konar.az.DAL;
using Konar.az.Helpers;
using Konar.az.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Konar.az.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public ProductsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;

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
            return View();
        }
        #endregion

        #region Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id,int[] tagsId,int brandId,int[] catId,Product product)
        {
            ViewBag.Brands = await _db.Brands.ToListAsync();
            ViewBag.Tags = await _db.Tags.ToListAsync();
            ViewBag.Categories = await _db.Categories.ToListAsync();


            #region İmages Download
            List<ProductImage> productImages = new List<ProductImage>();
            if (product.Photos == null)
            {
                ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil seçin");
                return View();
            }
            foreach (IFormFile photo in product.Photos)
            {
                if (!photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil faylı seçin");
                    return View();
                }
                if (photo.IsOlder2MB())
                {
                    ModelState.AddModelError("Photo", "Maksimum 2MB ölçüsündə fayl seçin");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "img");
                ProductImage productImage = new ProductImage
                {
                    Image = await photo.SaveImageAsync(folder),
                };
                productImages.Add(productImage);
            }
            product.ProductImages = productImages;
            #endregion


            #region Category
            List<ProductCategory> productCategories = new List<ProductCategory>();

            foreach (int categoryId in catId)
            {
                ProductCategory productCategory = new ProductCategory
                {
                    CategoryId = categoryId,

                };
                productCategories.Add(productCategory);
            }
            product.ProductCategories = productCategories;

            #endregion

            #region Tags
            List<ProductTag> productTags = new List<ProductTag>();

            foreach (int tagId in tagsId)
            {
                ProductTag productTag = new ProductTag
                {
                    TagId = tagId,

                };
                productTags.Add(productTag);
            }
            product.ProductTags = productTags;
            #endregion

            product.BrandId = brandId;

            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #endregion

        #region Update
        #region Get
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product? dbProduct=await _db.Products
                 .Include(x => x.ProductDetail)
                 .Include(x => x.ProductFeatures)
                 .Include(x => x.ProductImages)
                 .Include(x => x.Brand)
                 .Include(x => x.ProductCategories)
                 .ThenInclude(x => x.Category)
                 .Include(x => x.ProductTags)
                 .ThenInclude(x => x.Tag)
                .FirstOrDefaultAsync();
            if(dbProduct == null)
            {
                return BadRequest();
            }
            ViewBag.Brands = await _db.Brands.ToListAsync();
            ViewBag.Tags = await _db.Tags.ToListAsync();
            ViewBag.Categories = await _db.Categories.ToListAsync();
            return View(dbProduct);
        }
        #endregion
        #endregion


    }
}
