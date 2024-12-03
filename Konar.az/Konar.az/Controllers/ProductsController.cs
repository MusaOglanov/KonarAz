﻿using Konar.az.DAL;
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


		public async Task<IActionResult> Index(string categoryIds, string brandIds,string tagId)
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


			// Execute the query
			var products = await productsQuery.ToListAsync();

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






		//IQueryable<Product> products = _db.Products
		//	.Include(x => x.ProductDetail)
		//	.Include(x => x.ProductImages)
		//	.Include(x => x.Brand)
		//	.Include(x => x.ProductCategories)
		//	.ThenInclude(x => x.Category)
		//	.Include(x => x.ProductTags)
		//	.ThenInclude(x => x.Tag)
		//	.Include(x => x.ProductDetail);

		//// Eğer categoryId var ise, sadece o kategoriya ait olanları alırıq
		//if (categoryId.HasValue)
		//{
		//	products = products.Where(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryId));
		//}

		//// Eğer brandId var ise, sadece o brand-a ait olanları alırıq
		//if (brandId.HasValue)
		//{
		//	products = products.Where(p => p.BrandId == brandId);
		//}

		//// Eğer tagId var ise, sadece o tag-a aid olanları alırıq
		//if (tagId.HasValue)
		//{
		//	products = products.Where(p => p.ProductTags.Any(pt => pt.TagId == tagId));
		//}

		//List<Product> productList = await products.ToListAsync();

		//List<Tag> tags = await _db.Tags.ToListAsync();
		//List<Brand> brands = await _db.Brands.ToListAsync();
		//List<Category> categories = await _db.Categories.ToListAsync();

		//ViewBag.Tags = tags;
		//ViewBag.Brands = brands;
		//ViewBag.Categories = categories;

		//return View(productList);


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
//public async Task<List<Product>> GetWhereProduct(StockIndexVM model)
//{
//	IQueryable<Product> query = _productReadRepository.GetAll().Include(p => p.Brands)

//															 .Include(p => p.Category)
//															 .Include(p => p.Color)
//															 .Include(p => p.Wishlist);

//	if (model.ProductWhereDto != null)
//	{

//		query = query.Where(pr => (model.ProductWhereDto.CategoryId != null ? pr.CategoryId == Guid.Parse(model.ProductWhereDto.CategoryId) : true));
//		query = query.Where(pr => (model.ProductWhereDto.minValue != null ? pr.Price >= model.ProductWhereDto.minValue : true) && (model.ProductWhereDto.maxValue != null ? pr.Price <= model.ProductWhereDto.maxValue : true));
//		query = query.Where(pr => (model.ProductWhereDto.BrandsId != null ? pr.BrandsId == Guid.Parse(model.ProductWhereDto.BrandsId) : true));
//		query = query.Where(pr => (model.ProductWhereDto.ColorId != null ? pr.ColorId == Guid.Parse(model.ProductWhereDto.ColorId) : true));
//	}

//	return await query.ToListAsync();
//}