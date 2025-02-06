using Konar.az.DAL;
using Konar.az.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Konar.az.Controllers
{
    public class CaseStudiesController : Controller
    {
        private readonly AppDbContext _db;
        public CaseStudiesController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }




        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
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
    }
}
