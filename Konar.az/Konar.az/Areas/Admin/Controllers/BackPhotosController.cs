using Konar.az.DAL;
using Konar.az.Helpers;
using Konar.az.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Konar.az.Areas.Admin.Controllers
{

	[Area("Admin")]
	[Authorize(Roles = "Admin")]

	public class BackPhotosController : Controller
	{
		private readonly AppDbContext _db;
		private readonly IWebHostEnvironment _env;
		public BackPhotosController(AppDbContext db, IWebHostEnvironment env)
		{
			_db = db;
			_env = env;

		}
		public async Task<IActionResult> Index()
		{
			BackPhoto photo=await _db.BackPhotos.FirstOrDefaultAsync();
			return View(photo);
		}

		public async  Task<IActionResult>Update(int? id)
		{
			//if (id == null)

			//{
			//	return NotFound();
			//}
			BackPhoto dbBackPhoto = await _db.BackPhotos.FirstOrDefaultAsync(x => x.Id == id);
			//if (dbBackPhoto == null)
			//{
			//	return BadRequest();
			//}

			return View(dbBackPhoto);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(BackPhoto backPhoto, int? id)
		{
			//if (id == null)

			//{
			//	return NotFound();
			//}
			BackPhoto dbBackPhoto = await _db.BackPhotos.FirstOrDefaultAsync(x => x.Id == id);
			//if (dbBackPhoto == null)
			//{
			//	return BadRequest();
			//}
			if (backPhoto.ProductPhoto != null && backPhoto.FaqPhoto != null && backPhoto.BlogPhoto != null  )
			{
				if (!backPhoto.ProductPhoto.IsImage() && backPhoto.FaqPhoto.IsImage() && backPhoto.BlogPhoto.IsImage())
				{
					ModelState.AddModelError("Photo", "Zəhmət olmasa 'image' faylı seçin");

					return View();
				}
				if (!backPhoto.ProductPhoto.IsOlder2MB() && backPhoto.FaqPhoto.IsOlder2MB() && backPhoto.BlogPhoto.IsOlder2MB())
				{
					ModelState.AddModelError("Photo", "Maksimum 2MB ölçüsündə fayl seçin");

					return View();
				}
				string folder = Path.Combine(_env.WebRootPath, "img");
				Extensions.DeleteFile(folder, dbBackPhoto.ProductImage);
				dbBackPhoto.ProductImage = await backPhoto.ProductPhoto.SaveImageAsync(folder);

				Extensions.DeleteFile(folder, dbBackPhoto.BlogImage);
				dbBackPhoto.BlogImage = await backPhoto.BlogPhoto.SaveImageAsync(folder);

				Extensions.DeleteFile(folder, dbBackPhoto.FaqImage);
				dbBackPhoto.FaqImage = await backPhoto.FaqPhoto.SaveImageAsync(folder);
			}

			await _db.SaveChangesAsync();
			return RedirectToAction("Index");
		}
	}
}
