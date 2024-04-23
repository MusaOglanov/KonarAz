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
    public class SlidersController : Controller
	{
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public SlidersController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;

        }
        public async Task<ActionResult> Index()
		{
            List<Slider> sliders = await _db.Sliders.ToListAsync();
			return View(sliders);
		}

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (slider.Photo == null)
            {
                ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil seçin");
                return View();
            }
            if (!slider.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Zəhmət olmasa 'image' faylı seçin");

                return View();
            }
            if (slider.Photo.IsOlder2MB())
            {
                ModelState.AddModelError("Photo", "Maksimum 2MB ölçüsündə fayl seçin");

                return View();
            }
            string folder = Path.Combine(_env.WebRootPath, "img");
            slider.Image = await slider.Photo.SaveImageAsync(folder);


            await _db.Sliders.AddAsync(slider);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id)
        {
            if(id == null)
            {
                return BadRequest();
            }
            Slider dbSlider = await _db.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (dbSlider == null)
            {
                return NotFound();  
            }

            return View(dbSlider);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,Slider slider)
        {
            if(id == null)
            {
                return BadRequest();
            }
            Slider dbSlider = await _db.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (dbSlider == null)
            {
                return NotFound();  
            }

            if (slider.Photo != null)
            {
                if (!slider.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Zəhmət olmasa 'image' faylı seçin");

                    return View();
                }
                if (slider.Photo.IsOlder2MB())
                {
                    ModelState.AddModelError("Photo", "Maksimum 2MB ölçüsündə fayl seçin");

                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "img");
                Extensions.DeleteFile(folder, dbSlider.Image);
                slider.Image = await slider.Photo.SaveImageAsync(folder);
            }

            dbSlider.Title=slider.Title;
            dbSlider.SubTitle=slider.SubTitle;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
