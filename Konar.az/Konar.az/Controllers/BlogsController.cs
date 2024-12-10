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
		public async Task<IActionResult> Index(string categoryIds, string tagId,int page=1)
		{
			var categoryIdList = !string.IsNullOrEmpty(categoryIds)
				? categoryIds.Split(',').Select(id => int.Parse(id)).ToArray()
				: Array.Empty<int>();

			var blogsQuery = _db.Blogs
				.Include(b => b.BlogCategory)
				.Include(b => b.BlogTags)
				.ThenInclude(b => b.Tag)
				.AsQueryable();

			if (categoryIdList.Any())
			{
				blogsQuery = blogsQuery.Where(p => categoryIdList.Contains(p.BlogCategoryId));
			}

			if (!string.IsNullOrEmpty(tagId))
			{
				int tagIdParsed = int.Parse(tagId);
				blogsQuery = blogsQuery.Where(p => p.BlogTags.Any(t => t.Id == tagIdParsed));
			}

			int showCount = 3;

			var blogs = await blogsQuery.OrderByDescending(x=>x.Id).Skip((page - 1) * showCount).Take(showCount).ToListAsync();
			ViewBag.PageCount = Math.Ceiling((decimal)await _db.Blogs.CountAsync() / showCount);
			ViewBag.CurrentPage = page;

			ViewBag.Blogs = await _db.Blogs.Include(x => x.BlogCategory).ToListAsync();
			ViewBag.BlogCategories = await _db.BlogCategories.ToListAsync();
			ViewBag.Tags = await _db.Tags.ToListAsync();
			ViewBag.BackPhoto = await _db.BackPhotos.FirstOrDefaultAsync();
         


            return View(blogs);
        }
        public async Task<IActionResult> Detail (int? id)
        {
            if(id== null)
            {
                return NotFound();

            }
            Blog? dbBlog=await _db.Blogs
                .Include(x => x.BlogCategory)
                .Include(x => x.BlogTags)
                .FirstOrDefaultAsync(x => x.Id == id);
			if (dbBlog == null)
			{
				return BadRequest();

			}
			return View(dbBlog);
        }
    }
}
