using Konar.az.DAL;
using Konar.az.Models;
using Konar.az.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Konar.az.Controllers
{
	public class HomeController : Controller
	{
		private readonly AppDbContext _db;

		public HomeController(AppDbContext db)
		{
			_db = db;
		}
		public async Task<IActionResult> Index()
		{
			HomeVM homeVM = new HomeVM
			{
				Sliders = await _db.Sliders.ToListAsync(),
				CaseStudies = await _db.CaseStudies.ToListAsync(),
				Brands = await _db.Brands.ToListAsync(),
				Blogs = await _db.Blogs.Where(x => x.IsDeActive == false).Include(x => x.BlogCategory).ToListAsync(),
				Products = await _db.Products.Include(x=>x.ProductImages).ToListAsync(),
			};
            ViewBag.SlideVideo = await _db.HomeVideos.FirstOrDefaultAsync();

            return View(homeVM);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
