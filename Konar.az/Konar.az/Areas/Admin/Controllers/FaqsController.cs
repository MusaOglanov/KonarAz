using Konar.az.DAL;
using Konar.az.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Konar.az.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FaqsController : Controller
    {
        private readonly AppDbContext _db;
        public FaqsController(AppDbContext db)
        {
            _db = db;

        }
        public async Task<IActionResult> Index(int page = 1)
        {
			int showCount = 8;

			ViewBag.PageCount = Math.Ceiling((decimal)await _db.Faqs.CountAsync() / showCount);
			ViewBag.CurrentPage = page;
			List<Faq> faqs = await _db.Faqs.OrderByDescending(x => x.Id).Skip((page - 1) * showCount).Take(showCount).ToListAsync();
            return View(faqs);
        }

        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Faq faq)
        {
            if (faq.Answer == null)
            {
                ModelState.AddModelError("Answer", "Boş ola bilməz");
                return View();
            }
            if (faq.Question == null)
            {
                ModelState.AddModelError("Question", "Boş ola bilməz");
                return View();
            }
            await _db.Faqs.AddAsync(faq);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            Faq? dbFaq = await _db.Faqs
                .FirstOrDefaultAsync(x => x.Id == id);
            if (dbFaq == null)
            {
                return BadRequest();
            }
            return View(dbFaq);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Faq faq, int? id)
        {
            if (faq.Answer == null)
            {
                ModelState.AddModelError("Answer", "Boş ola bilməz");
                return View();
            }
            if (faq.Question == null)
            {
                ModelState.AddModelError("Question", "Boş ola bilməz");
                return View();
            }
            if (id == null)
            {
                return NotFound();
            }
            Faq? dbFaq = await _db.Faqs
                .FirstOrDefaultAsync(x => x.Id == id);
            if (dbFaq == null)
            {
                return BadRequest();
            }
            dbFaq.Answer = faq.Answer;
            dbFaq.Question = faq.Question;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Faq dbFaq= await _db.Faqs.FirstOrDefaultAsync(t => t.Id == id);
            if (dbFaq == null)
            {
                return BadRequest();
            }

            if (dbFaq.IsDeactive)
            {
                dbFaq.IsDeactive = false;
            }
            else
            {
                dbFaq.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
