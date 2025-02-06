using Konar.az.DAL;
using Konar.az.Helpers;
using Konar.az.Models;
using Konar.az.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Konar.az.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class BrandsController : Controller
	{
		private readonly AppDbContext _db;
		private readonly IWebHostEnvironment _env;
		public BrandsController(AppDbContext db, IWebHostEnvironment env)
		{
			_db = db;
			_env = env;
		}
		public async Task<IActionResult> Index(int page = 1)
		{
			int showCount = 8;

			ViewBag.PageCount = Math.Ceiling((decimal)await _db.Brands.CountAsync() / showCount);
			ViewBag.CurrentPage = page;
			List<Brand> brands =await _db.Brands.OrderByDescending(x => x.Id).Skip((page - 1) * showCount).Take(showCount).ToListAsync();
			return View(brands);
		}


		public async Task<IActionResult> Create()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Brand brand)
		{
			if (brand.Photo == null)
			{
				ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil seçin");
				return View();
			}
			if (!brand.Photo.IsImage())
			{
				ModelState.AddModelError("Photo", "Zəhmət olmasa 'image' faylı seçin");

				return View(brand);
			}
			if (brand.Photo.IsOlder2MB())
			{
				ModelState.AddModelError("Photo", "Maksimum 2MB ölçüsündə fayl seçin");

				return View(brand);
			}
			string imgFolder = Path.Combine(_env.WebRootPath, "img");
			brand.Image = await brand.Photo.SaveImageAsync(imgFolder);

			await _db.Brands.AddAsync(brand);
			await _db.SaveChangesAsync();
			return RedirectToAction("Index");
		}

        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Brand dbBrand = await _db.Brands.FirstOrDefaultAsync(t => t.Id == id);
            if (dbBrand == null)
            {
                return BadRequest();
            }

            if (dbBrand.IsDeactive)
            {
                dbBrand.IsDeactive = false;
            }
            else
            {
                dbBrand.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


    }
}
