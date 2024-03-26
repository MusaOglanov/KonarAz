using Konar.az.DAL;
using Konar.az.Models;
using Konar.az.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Konar.az.Controllers
{
    public class BlogsController : Controller
    {
        private readonly AppDbContext _db;
        public BlogsController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            BlogVM blogVM = new BlogVM
            {
                Blogs = await _db.Blogs.Include(x => x.BlogCategory).ToListAsync(),
                BlogCategories = await _db.BlogCategories.ToListAsync(),
                Tags = await _db.Tags.ToListAsync(),
            };


            return View(blogVM);
        }
    }
}
