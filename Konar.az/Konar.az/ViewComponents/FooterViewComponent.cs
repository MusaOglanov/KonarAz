using Konar.az.DAL;
using Konar.az.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Konar.az.ViewComponents
{
	public class FooterViewComponent:ViewComponent
	{
		private readonly AppDbContext _db;
		public FooterViewComponent(AppDbContext db)
		{
			_db = db;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			Bio bio = await _db.Bios.FirstOrDefaultAsync();
			return View(bio);
		}
	}
}
