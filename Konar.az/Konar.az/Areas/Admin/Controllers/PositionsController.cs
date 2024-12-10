using Konar.az.DAL;
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
    public class PositionsController : Controller
    {
        private readonly AppDbContext _db;
        public PositionsController(AppDbContext db)
        {
            _db = db;

        }

        public async Task<IActionResult> Index(int page = 1)
        {
			int showCount = 8;

			ViewBag.PageCount = Math.Ceiling((decimal)await _db.Positions.CountAsync() / showCount);
			ViewBag.CurrentPage = page;
			List<Position> positions = await _db.Positions.OrderByDescending(x => x.Id).Skip((page - 1) * showCount).Take(showCount)
				.ToListAsync();
            return View(positions);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Position position)
        {
            if (position.Name == null)
            {
                ModelState.AddModelError("Name", "Boş ola bilməz");
                return View();
            }
            bool isExist = await _db.Positions.AnyAsync(x => x.Name == position.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu adda Vəzifə daha əvvəl istifadə olunub");
                return View();
            }
            await _db.Positions.AddAsync(position);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Position? dbPosition = await _db.Positions.FirstOrDefaultAsync(x => x.Id == id);
            if (dbPosition == null)
            {
                return BadRequest();
            }
            return View(dbPosition);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Position position, int? id)
        {
            if (position.Name == null)
            {
                ModelState.AddModelError("Name", "Boş ola bilməz");
                return View();
            }
            if (id == null)
            {
                return NotFound();
            }
            Position? dbPosition = await _db.Positions.FirstOrDefaultAsync(x => x.Id == id);
            if (dbPosition == null)
            {
                return BadRequest();
            }
            bool isExist = await _db.Positions.AnyAsync(x => x.Name == position.Name && x.Id!=id);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu adda Vəzifə daha əvvəl istifadə olunub");
                return View();
            }
            dbPosition.Name = position.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
