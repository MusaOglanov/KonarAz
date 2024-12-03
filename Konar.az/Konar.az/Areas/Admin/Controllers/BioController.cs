using Konar.az.DAL;
using Konar.az.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Konar.az.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BioController : Controller
    {
        private readonly AppDbContext _db;
        public BioController(AppDbContext db)
        {
            _db = db;

        }
        public async Task< IActionResult> Index()
		{
            Bio bio= await _db.Bios.FirstOrDefaultAsync();
			return View(bio);
		}


        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Bio dbBio =await _db.Bios.FirstOrDefaultAsync(x => x.Id == id);
            if (dbBio == null)
            {
                return BadRequest();
            }

            return View(dbBio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Bio bio,int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Bio dbBio = await _db.Bios.FirstOrDefaultAsync(x=>x.Id==id);
            if (dbBio == null)
            {
                return BadRequest();
            }
            dbBio.Adres= bio.Adres;
            dbBio.Tel= bio.Tel;
            dbBio.Mail= bio.Mail;
            dbBio.Facebook= bio.Facebook;
            dbBio.Instagram= bio.Instagram;
            dbBio.Linkedin= bio.Linkedin;
            dbBio.Whatsapp= bio.Whatsapp;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
