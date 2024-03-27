using Konar.az.DAL;
using Konar.az.Models;
using Konar.az.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Konar.az.Controllers
{
	public class AboutController : Controller
	{
		private readonly AppDbContext _db;

		public AboutController(AppDbContext db)
		{
			_db = db;
		}
		public async Task<IActionResult> Index()
		{


			AboutVM aboutVM = new AboutVM
			{
				About = await _db.Abouts.FirstOrDefaultAsync(),
				Statistics = await _db.Statistics.FirstOrDefaultAsync(),

			};
			return View(aboutVM);
		}
	}
}
