using Konar.az.DAL;
using Konar.az.Helpers;
using Konar.az.Models;
using Konar.az.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        public async Task<ActionResult> Index(int page = 1)
        {
            int showCount = 3;

            ViewBag.PageCount = Math.Ceiling((decimal)await _db.Sliders.CountAsync() / showCount);
            ViewBag.CurrentPage = page;
            List<Slider> sliders = await _db.Sliders.OrderByDescending(x => x.Id).Skip((page - 1) * showCount).Take(showCount).ToListAsync();
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
            if (id == null)
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
        public async Task<IActionResult> Update(int? id, Slider slider)
        {
            if (id == null)
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
                dbSlider.Image = await slider.Photo.SaveImageAsync(folder);
            }

            dbSlider.Title = slider.Title;
            dbSlider.SubTitle = slider.SubTitle;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Slider dbSlider = await _db.Sliders.FirstOrDefaultAsync(t => t.Id == id);
            if (dbSlider == null)
            {
                return BadRequest();
            }

            if (dbSlider.IsDeactive)
            {
                dbSlider.IsDeactive = false;
            }
            else
            {
                dbSlider.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> CreateVideo(int? id)
        {
           
            HomeVideo? dbHomeVideo = await _db.HomeVideos.FirstOrDefaultAsync();
           
            return View(dbHomeVideo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVideo(HomeVideo slideVideo)
        {

            if (slideVideo.Mp4 != null)
            {
                HomeVideo? dbHomeVideo = await _db.HomeVideos.FirstOrDefaultAsync();
                if (!slideVideo.Mp4.IsVideo())
                {
                    ModelState.AddModelError("Mp4", "Zəhmət olmasa 'video' faylı seçin");
                    return View(dbHomeVideo);
                }

                string folder = Path.Combine(_env.WebRootPath, "img");
                Extensions.DeleteFile(folder, dbHomeVideo.SlideVideo);
                dbHomeVideo.SlideVideo = await slideVideo.Mp4.SaveVideoAsync(folder);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }



    }
}
