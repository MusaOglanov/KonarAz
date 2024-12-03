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
	public class CaseStudiesController : Controller
	{
		private readonly AppDbContext _db;
		private readonly IWebHostEnvironment _env;
		public CaseStudiesController(AppDbContext db, IWebHostEnvironment env)
		{
			_db = db;
			_env = env;

		}
		public async Task<IActionResult> Index()
		{
			List<CaseStudy> studies=await _db.CaseStudies.ToListAsync();
			return View(studies);
		}

		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CaseStudy caseStudy)
		{
			if (caseStudy.Photo == null)
			{
				ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil seçin");
				return View();
			}
			if (!caseStudy.Photo.IsImage())
			{
				ModelState.AddModelError("Photo", "Zəhmət olmasa 'image' faylı seçin");

				return View();
			}
			if (caseStudy.Photo.IsOlder2MB())
			{
				ModelState.AddModelError("Photo", "Maksimum 2MB ölçüsündə fayl seçin");

				return View();
			}
			string folder = Path.Combine(_env.WebRootPath, "img");
			caseStudy.Image = await caseStudy.Photo.SaveImageAsync(folder);

			await _db.CaseStudies.AddAsync(caseStudy);
			await _db.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Update(int? id)
		{
			if(id == null)
			{
				return NotFound();
			}
			CaseStudy dbCaseStudy=await _db.CaseStudies.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCaseStudy == null)
			{
				return BadRequest();
			}

			return View(dbCaseStudy);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int? id,CaseStudy caseStudy)
		{
			if(id == null)
			{
				return NotFound();
			}
			CaseStudy dbCaseStudy=await _db.CaseStudies.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCaseStudy == null)
			{
				return BadRequest();
			}

			if (caseStudy.Photo != null)
			{
				if (!caseStudy.Photo.IsImage())
				{
					ModelState.AddModelError("Photo", "Zəhmət olmasa 'image' faylı seçin");

					return View(dbCaseStudy);
				}
				if (caseStudy.Photo.IsOlder2MB())
				{
					ModelState.AddModelError("Photo", "Maksimum 2MB ölçüsündə fayl seçin");

					return View(dbCaseStudy);
				}
				string folder = Path.Combine(_env.WebRootPath, "img");
				Extensions.DeleteFile(folder, dbCaseStudy.Image);
				dbCaseStudy.Image = await caseStudy.Photo.SaveImageAsync(folder);
			}


			dbCaseStudy.Title = caseStudy.Title;
			dbCaseStudy.SubTitle = caseStudy.SubTitle;
			await _db.SaveChangesAsync();
			return RedirectToAction("Index");
		}
	}
}
