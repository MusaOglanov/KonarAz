using Konar.az.DAL;
using Konar.az.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Konar.az.Controllers
{
	public class FaqsController : Controller
	{
		private readonly AppDbContext _db;
        public FaqsController(AppDbContext db)
        {
            _db=db;
        }
        public async Task<IActionResult> Index()
		{
			List<Faq> faqs = await _db.Faqs.ToListAsync();
			return View(faqs);
		}
	}
}
