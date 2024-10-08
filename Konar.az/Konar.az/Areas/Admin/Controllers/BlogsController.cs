using Konar.az.DAL;
using Konar.az.Helpers;
using Konar.az.Models;
using Konar.az.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using static NuGet.Packaging.PackagingConstants;

namespace Konar.az.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]

	public class BlogsController : Controller
	{
		private readonly AppDbContext _db;
		private readonly IWebHostEnvironment _env;
		public BlogsController(AppDbContext db, IWebHostEnvironment env)
		{
			_db = db;
			_env = env;

		}
		public async Task<IActionResult> Index()
		{
			List<Blog> blog = await _db.Blogs.Include(x => x.BlogCategory).ToListAsync();
			return View(blog);
		}

		#region Create

		#region Get
		public async Task<IActionResult> Create()
		{
			ViewBag.BlogCategories = await _db.BlogCategories.ToListAsync();
			ViewBag.Tags = await _db.Tags.ToListAsync();
			return View();
		}
		#endregion

		#region Post
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Blog blog, int[] tagsId, int catId)
		{

			ViewBag.BlogCategories = await _db.BlogCategories.ToListAsync();
			ViewBag.Tags = await _db.Tags.ToListAsync();

			if (blog.Photo == null)
			{
				ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil seçin");
				return View();
			}
			if (!blog.Photo.IsImage())
			{
				ModelState.AddModelError("Photo", "Zəhmət olmasa 'image' faylı seçin");

				return View();
			}
			if (blog.Photo.IsOlder2MB())
			{
				ModelState.AddModelError("Photo", "Maksimum 2MB ölçüsündə fayl seçin");

				return View();
			}
			string folder = Path.Combine(_env.WebRootPath, "img");
			blog.Image = await blog.Photo.SaveImageAsync(folder);


			List<BlogTag> blogTags = new List<BlogTag>();

			foreach (int tagId in tagsId)
			{
				BlogTag blogTag = new BlogTag
				{
					TagId = tagId,

				};
				blogTags.Add(blogTag);
			}
			if (blog.BlogCategoryId == null)
			{
				ModelState.AddModelError("BlogCategory", "Kategoriya seçin və ya yoxdursa yenisini yaradın");
				return View();
			}
			blog.BlogCategoryId = catId;

			blog.BlogTags = blogTags;
			blog.CreateTime = DateTime.Now;
			await _db.Blogs.AddAsync(blog);
			await _db.SaveChangesAsync();
			return RedirectToAction("Index");
		}
		#endregion

		#endregion


		#region Update

		#region Get
		public async Task<IActionResult> Update(int? id)
		{
			ViewBag.BlogCategories = await _db.BlogCategories.ToListAsync();
			ViewBag.Tags = await _db.Tags.ToListAsync();

			if (id == null)
			{
				return NotFound();
			}
			Blog? dbBlog = await _db.Blogs.Include(x => x.BlogCategory)
				.Include(x => x.BlogTags)
				.ThenInclude(x => x.Tag)
				.FirstOrDefaultAsync(x => x.Id == id);
			if (dbBlog == null)
			{
				return BadRequest();
			}

			return View(dbBlog);
		}
		#endregion

		#region Post
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int? id, Blog blog, int[] tagsId, int catId)
		{


			if (id == null)
			{
				return NotFound();
			}
			Blog? dbBlog = await _db.Blogs
				.Include(x => x.BlogCategory)
				.Include(x => x.BlogTags)
				.ThenInclude(x => x.Tag)
				.FirstOrDefaultAsync(x => x.Id == id);
			if (dbBlog == null)
			{
				return BadRequest();
			}

			ViewBag.BlogCategories = await _db.BlogCategories.ToListAsync();
			ViewBag.Tags = await _db.Tags.ToListAsync();

			if (blog.Photo != null)
			{
				if (!blog.Photo.IsImage())
				{
					ModelState.AddModelError("Photo", "Zəhmət olmasa 'image' faylı seçin");

					return View(dbBlog);
				}
				if (blog.Photo.IsOlder2MB())
				{
					ModelState.AddModelError("Photo", "Maksimum 2MB ölçüsündə fayl seçin");

					return View(dbBlog);
				}
				string folder = Path.Combine(_env.WebRootPath, "img");
				Extensions.DeleteFile(folder, dbBlog.Image);
				dbBlog.Image = await blog.Photo.SaveImageAsync(folder);
			}
			List<BlogTag> blogTags = new List<BlogTag>();
			foreach (int tagId in tagsId)
			{
				BlogTag blogTag = new BlogTag
				{
					TagId = tagId,

				};
				blogTags.Add(blogTag);
			}
			dbBlog.BlogTags = blogTags;



			dbBlog.BlogCategoryId = catId;
			dbBlog.Title = blog.Title;
			dbBlog.SubTitle = blog.SubTitle;
			await _db.SaveChangesAsync();
			return RedirectToAction("Index");
		}
		#endregion

		#endregion
	}
}
