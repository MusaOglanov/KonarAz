using Konar.az.DAL;
using Konar.az.Helpers;
using Konar.az.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Composition;
using System.Drawing;
using System.Globalization;

namespace Konar.az.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class AboutController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public AboutController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<About> about = await _db.Abouts.ToListAsync();
            return View(about);
        }


        #region Create

        #region get
        public IActionResult Create()
        {
            return View();
        }
        #endregion

        #region post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(About about)
        {
            if (about.Photo == null)
            {
                ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil seçin");
                return View();
            }
            if (!about.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Zəhmət olmasa 'image' faylı seçin");

                return View(about);
            }
            if (about.Photo.IsOlder2MB())
            {
                ModelState.AddModelError("Photo", "Maksimum 2MB ölçüsündə fayl seçin");

                return View(about);
            }
            string imgFolder = Path.Combine(_env.WebRootPath, "img");
            about.Image = await about.Photo.SaveImageAsync(imgFolder);

            if (about.Mp4 == null)
            {
                ModelState.AddModelError("Mp4", "Zəhmət olmasa şəkil seçin");
                return View();
            }
            if (!about.Mp4.IsVideo())
            {
                ModelState.AddModelError("Mp4", "Zəhmət olmasa 'image' faylı seçin");

                return View(about);
            }

            string mpd4Folder = Path.Combine(_env.WebRootPath, "img");
            about.Video = await about.Mp4.SaveVideoAsync(mpd4Folder);

            await _db.Abouts.AddAsync(about);
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
            About? dbAbout = await _db.Abouts.FirstOrDefaultAsync(x => x.Id == id);
            if (dbAbout == null)
            {
                return BadRequest();
            }
            return View(dbAbout);
        }
        #endregion

        #region Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, About about)
        {
            if (id == null)
            {
                return NotFound();
            }
            About? dbAbout = await _db.Abouts.FirstOrDefaultAsync(x => x.Id == id);
            if (dbAbout == null)
            {
                return BadRequest();
            }

            if (about.Photo != null)
            {
                if (!about.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Zəhmət olmasa 'image' faylı seçin");

                    return View(dbAbout);
                }
                if (about.Photo.IsOlder2MB())
                {
                    ModelState.AddModelError("Photo", "Maksimum 2MB ölçüsündə fayl seçin");

                    return View(dbAbout);
                }
                string folder = Path.Combine(_env.WebRootPath, "img");
                Extensions.DeleteFile(folder, dbAbout.Image);
                dbAbout.Image = await about.Photo.SaveImageAsync(folder);
            }
            if (about.Mp4 != null)
            {
                if (!about.Mp4.IsVideo())
                {
                    ModelState.AddModelError("Mp4", "Zəhmət olmasa 'video' faylı seçin");
                    return View(dbAbout);
                }

                string folder = Path.Combine(_env.WebRootPath, "img");
                Extensions.DeleteFile(folder, dbAbout.Video);
                dbAbout.Video = await about.Mp4.SaveVideoAsync(folder);
            }
            dbAbout.Title = about.Title;
            dbAbout.SubTitle = about.SubTitle;
            dbAbout.Title = about.Title;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #endregion


    }
}
