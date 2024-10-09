using Konar.az.DAL;
using Konar.az.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Konar.az.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _db;
        public CategoriesController(AppDbContext db)
        {
            _db = db;

        }
        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _db.Categories.ToListAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (category.Name == null)
            {
                ModelState.AddModelError("Name", "Boş ola bilməz");
                return View();
            }
            bool isExist = await _db.Categories.AnyAsync(x => x.Name == category.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu adda Vəzifə daha əvvəl istifadə olunub");
                return View();
            }
            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Category dbCategory=await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCategory == null) 
            {
                return NotFound();
            }


            return View(dbCategory);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,Category category)
        {
            if (category.Name == null)
            {
                ModelState.AddModelError("Name", "Boş ola bilməz");
                return View();
            }
            if (id == null)
            {
                return BadRequest();
            }
            Category dbCategory = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCategory == null)
            {
                return NotFound();
            }
            bool isExist = await _db.Categories.AnyAsync(x => x.Name == category.Name && x.Id != id);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu adda Vəzifə daha əvvəl istifadə olunub");
                return View();
            }
            dbCategory.Name = category.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
