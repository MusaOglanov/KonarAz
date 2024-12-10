using Konar.az.DAL;
using Konar.az.Helpers;
using Konar.az.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Drawing;

namespace Konar.az.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

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
        public async Task<IActionResult> Index(int page=1)
        {
			int showCount = 8;

			ViewBag.PageCount = Math.Ceiling((decimal)await _db.Products.CountAsync() / showCount);
			ViewBag.CurrentPage = page;
			List<Product> product = await _db.Products
                 .Include(x => x.ProductDetail)
                 .Include(x => x.ProductFeatures)
                 .Include(x => x.ProductImages)
                 .Include(x => x.Brand)
                 .Include(x => x.ProductCategories)
                 .ThenInclude(x => x.Category)
                 .Include(x => x.ProductTags)
                 .ThenInclude(x => x.Tag).OrderByDescending(x => x.Id).Skip((page - 1) * showCount).Take(showCount)
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
        public async Task<IActionResult> Create(int? id, int[] tagsId, int brandId, int[] catId, Product product)
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
            Product? dbProduct = await _db.Products
                 .Include(x => x.ProductDetail)
                 .Include(x => x.ProductFeatures)
                 .Include(x => x.ProductImages)
                 .Include(x => x.ProductCategories)
                 .ThenInclude(x => x.Category)
                 .Include(x => x.ProductTags)
                 .ThenInclude(x => x.Tag)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (dbProduct == null)
            {
                return BadRequest();
            }
            ViewBag.Brands = await _db.Brands.ToListAsync();
            ViewBag.Tags = await _db.Tags.ToListAsync();
            ViewBag.Categories = await _db.Categories.ToListAsync();
            return View(dbProduct);
        }
        #endregion

        #region Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, int[] tagsId, int brandId, int[] catId, Product product, List<ProductFeature> ProductFeatures)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product? dbProduct = await _db.Products
                 .Include(x => x.ProductDetail)
                 .Include(x => x.Brand)
                 .Include(x => x.ProductFeatures)
                 .Include(x => x.ProductImages)
                 .Include(x => x.ProductCategories)
                 .ThenInclude(x => x.Category)
                 .Include(x => x.ProductTags)
                 .ThenInclude(x => x.Tag)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (dbProduct == null)
            {
                return BadRequest();
            }
            ViewBag.Brands = await _db.Brands.ToListAsync();
            ViewBag.Tags = await _db.Tags.ToListAsync();
            ViewBag.Categories = await _db.Categories.ToListAsync();

            #region İmages Download
            List<ProductImage> productImages = new List<ProductImage>();

            if (product != null)
            {
                if (product.Photos != null && product.Photos.Count > 0)
                {
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

                    dbProduct.ProductImages.AddRange(productImages);
                }
            }


            #endregion

            // Məhsulun xüsusiyyətlərini yenilə
            #region Product Features Update
            if (ProductFeatures != null)
            {
                // Mövcud xüsusiyyətləri yenilə və ya sil
                var existingFeatures = dbProduct.ProductFeatures.ToList();
                foreach (var feature in existingFeatures)
                {
                    var updatedFeature = ProductFeatures.FirstOrDefault(f => f.Name == feature.Name);
                    if (updatedFeature != null)
                    {
                        // Mövcud xüsusiyyət yenilənir
                        feature.Value = updatedFeature.Value;
                        ProductFeatures.Remove(updatedFeature);  // Artıq işləndiyi üçün siyahıdan çıxarırıq
                    }
                    else
                    {
                        // Əgər formda xüsusiyyət yoxdursa, onu silirik
                        dbProduct.ProductFeatures.Remove(feature);
                    }
                }

                // Yeni xüsusiyyətlər əlavə et
                foreach (var feature in ProductFeatures)
                {
                    if (!string.IsNullOrEmpty(feature.Name) && !string.IsNullOrEmpty(feature.Value))
                    {
                        dbProduct.ProductFeatures.Add(new ProductFeature
                        {
                            Name = feature.Name,
                            Value = feature.Value
                        });
                    }
                }
            }
            #endregion


            dbProduct.BrandId = brandId;


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
            dbProduct.ProductCategories = productCategories;

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
            dbProduct.ProductTags = productTags;

           
            #endregion
            dbProduct.Name = product.Name;
            dbProduct.Price = product.Price;
            dbProduct.ProductDetail.Description = product.ProductDetail.Description;
            dbProduct.ProductDetail.Material = product.ProductDetail.Material;
            dbProduct.ProductDetail.HasStock = product.ProductDetail.HasStock;
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");

        }


        #endregion

        #endregion

        public async Task<IActionResult> DeleteImage(int proImageId)
        {
            ProductImage? productImage = await _db.ProductImages
                .FirstOrDefaultAsync(x => x.Id == proImageId);
            string folder = Path.Combine(_env.WebRootPath, "img");
            Extensions.DeleteFile(folder, productImage.Image);
            _db.ProductImages.Remove(productImage);
            _db.SaveChanges();
            return Ok();
        }

    }
}
