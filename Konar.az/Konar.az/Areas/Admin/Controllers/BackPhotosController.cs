using Konar.az.DAL;
using Konar.az.Helpers;
using Konar.az.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

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
			if (id == null)

			{
				return NotFound();
			}
			BackPhoto dbBackPhoto = await _db.BackPhotos.FirstOrDefaultAsync(x => x.Id == id);
			if (dbBackPhoto == null)
			{
				return BadRequest();
			}

			return View(dbBackPhoto);
		}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(BackPhoto backPhoto, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BackPhoto dbBackPhoto = await _db.BackPhotos.FirstOrDefaultAsync(x => x.Id == id);
            if (dbBackPhoto == null)
            {
                return BadRequest();
            }

            // Şəkil yükləmək üçün kök qovluq
            string folder = Path.Combine(_env.WebRootPath, "img");

            // Əgər `ProductPhoto` yüklənibsə, onu yenilə
            if (backPhoto.ProductPhoto != null)
            {
                if (!backPhoto.ProductPhoto.IsImage())
                {
                    ModelState.AddModelError("ProductPhoto", "Zəhmət olmasa 'image' faylı seçin.");
                    return View(dbBackPhoto);
                }

                if (backPhoto.ProductPhoto.IsOlder2MB())
                {
                    ModelState.AddModelError("ProductPhoto", "Maksimum 2MB ölçüsündə fayl seçin.");
                    return View(dbBackPhoto);
                }

                // Əvvəlki şəkli sil
                Extensions.DeleteFile(folder, dbBackPhoto.ProductImage);

                // Yeni şəkli yadda saxla
                dbBackPhoto.ProductImage = await backPhoto.ProductPhoto.SaveImageAsync(folder);
            }

            // Əgər `BlogPhoto` yüklənibsə, onu yenilə
            if (backPhoto.BlogPhoto != null)
            {
                if (!backPhoto.BlogPhoto.IsImage())
                {
                    ModelState.AddModelError("BlogPhoto", "Zəhmət olmasa 'image' faylı seçin.");
                    return View(dbBackPhoto);
                }

                if (backPhoto.BlogPhoto.IsOlder2MB())
                {
                    ModelState.AddModelError("BlogPhoto", "Maksimum 2MB ölçüsündə fayl seçin.");
                    return View(dbBackPhoto);
                }

                // Əvvəlki şəkli sil
                Extensions.DeleteFile(folder, dbBackPhoto.BlogImage);

                // Yeni şəkli yadda saxla
                dbBackPhoto.BlogImage = await backPhoto.BlogPhoto.SaveImageAsync(folder);
            }

            // Əgər `FaqPhoto` yüklənibsə, onu yenilə
            if (backPhoto.FaqPhoto != null)
            {
                if (!backPhoto.FaqPhoto.IsImage())
                {
                    ModelState.AddModelError("FaqPhoto", "Zəhmət olmasa 'image' faylı seçin.");
                    return View(dbBackPhoto);
                }

                if (backPhoto.FaqPhoto.IsOlder2MB())
                {
                    ModelState.AddModelError("FaqPhoto", "Maksimum 2MB ölçüsündə fayl seçin.");
                    return View(dbBackPhoto);
                }

                // Əvvəlki şəkli sil
                Extensions.DeleteFile(folder, dbBackPhoto.FaqImage);

                // Yeni şəkli yadda saxla
                dbBackPhoto.FaqImage = await backPhoto.FaqPhoto.SaveImageAsync(folder);
            }

            // Əgər `AccountPhoto` yüklənibsə, onu yenilə
            if (backPhoto.AccountPhoto != null)
            {
                if (!backPhoto.AccountPhoto.IsImage())
                {
                    ModelState.AddModelError("AccountPhoto", "Zəhmət olmasa 'image' faylı seçin.");
                    return View(dbBackPhoto);
                }

                if (backPhoto.AccountPhoto.IsOlder2MB())
                {
                    ModelState.AddModelError("AccountPhoto", "Maksimum 2MB ölçüsündə fayl seçin.");
                    return View(dbBackPhoto);
                }

                // Əvvəlki şəkli sil
                Extensions.DeleteFile(folder, dbBackPhoto.AccountImage);

                // Yeni şəkli yadda saxla
                dbBackPhoto.AccountImage = await backPhoto.AccountPhoto.SaveImageAsync(folder);
            }
              // Əgər `ContactPhoto` yüklənibsə, onu yenilə
            if (backPhoto.ContactPhoto != null)
            {
                if (!backPhoto.ContactPhoto.IsImage())
                {
                    ModelState.AddModelError("ContactPhoto", "Zəhmət olmasa 'image' faylı seçin.");
                    return View(dbBackPhoto);
                }

                if (backPhoto.ContactPhoto.IsOlder2MB())
                {
                    ModelState.AddModelError("ContactPhoto", "Maksimum 2MB ölçüsündə fayl seçin.");
                    return View(dbBackPhoto);
                }

                // Əvvəlki şəkli sil
                Extensions.DeleteFile(folder, dbBackPhoto.ContactImage);

                // Yeni şəkli yadda saxla
                dbBackPhoto.ContactImage = await backPhoto.ContactPhoto.SaveImageAsync(folder);
            }

            // Dəyişiklikləri yadda saxla
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
