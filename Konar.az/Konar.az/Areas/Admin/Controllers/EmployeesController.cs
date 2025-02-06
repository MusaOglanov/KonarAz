using Konar.az.DAL;
using Konar.az.Helpers;
using Konar.az.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Composition;

namespace Konar.az.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class EmployeesController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public EmployeesController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;

        }
        public async Task<IActionResult> Index(int page = 1)
        {
			int showCount = 8;

			ViewBag.PageCount = Math.Ceiling((decimal)await _db.Employee.CountAsync() / showCount);
			ViewBag.CurrentPage = page;
			List<Employee> employees = await _db.Employee
                .Include(x => x.Position)
				.OrderByDescending(x => x.Id).Skip((page - 1) * showCount).Take(showCount)
				.ToListAsync();
            return View(employees);
        }

        public async Task<IActionResult> Create()
		{
			int showCount = 8;

		
			ViewBag.Positions = await _db.Positions.ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee, int posId)
        {
            ViewBag.Positions = await _db.Positions.ToListAsync();

            bool isExist = await _db.Employee.AnyAsync(x => x.FullName == employee.FullName);

            if (isExist)
            {
                ModelState.AddModelError("FullName", "Bu adda İşçi daha əvvəl qeyd olunub");
                return View();
            }
            if (employee.Photo == null)
            {
                ModelState.AddModelError("Photo", "Zəhmət olmasa Şəkil seçin");
                return View();
            }
            if (!employee.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Zəhmət olmasa yalnız Şəkil faylı seçin");
                return View();
            }
            if (employee.Photo.IsOlder2MB())
            {
                ModelState.AddModelError("Photo", "Maksimum 2mb");
                return View();
            }
            string folder = Path.Combine(_env.WebRootPath, "img");
            employee.Image = await employee.Photo.SaveImageAsync(folder);
            employee.PositionId = posId;
            await _db.Employee.AddAsync(employee);
            await _db.SaveChangesAsync();


            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Positions = await _db.Positions.ToListAsync();
            if (id == null)
            {
                return NotFound();
            }
            Employee? dbEmployee = await _db.Employee
                .Include(x => x.Position)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (dbEmployee == null)
            {
                return BadRequest();
            }
            return View(dbEmployee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Employee employee, int posId, int? id)
        {
            ViewBag.Positions = await _db.Positions.ToListAsync();
            if (id == null)
            {
                return NotFound();
            }
            Employee? dbEmployee = await _db.Employee
                .Include(x => x.Position)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (dbEmployee == null)
            {
                return BadRequest();
            }
            if (employee.Photo != null)
            {
                if (!employee.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Zəhmət olmasa yalnız Şəkil faylı seçin");
                    return View(dbEmployee);
                }
                if (employee.Photo.IsOlder2MB())
                {
                    ModelState.AddModelError("Photo", "Maksimum 2mb");
                    return View(dbEmployee);
                }
                string folder = Path.Combine(_env.WebRootPath, "img");
                Extensions.DeleteFile(folder, dbEmployee.Image);
                dbEmployee.Image = await employee.Photo.SaveImageAsync(folder);
            }

            dbEmployee.PositionId = posId;
            dbEmployee.FullName = employee.FullName;

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employee dbEmployee = await _db.Employee.FirstOrDefaultAsync(t => t.Id == id);
            if (dbEmployee == null)
            {
                return BadRequest();
            }

            if (dbEmployee.IsDeactive)
            {
                dbEmployee.IsDeactive = false;
            }
            else
            {
                dbEmployee.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
