using Konar.az.DAL;
using Konar.az.Helpers;
using Konar.az.Models;
using Konar.az.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Konar.az.Controllers
{
	
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signManager;
        public AccountController(UserManager<AppUser> userManager
            ,RoleManager<IdentityRole> roleManager
            , SignInManager<AppUser> signManager,
            AppDbContext db)
        {
			_userManager=userManager;
			_roleManager = roleManager;
			_signManager = signManager;
			_db = db;

		}
		#region Login

		#region get
		public async Task<IActionResult> Login()
		{
            ViewBag.BackPhoto = await _db.BackPhotos.FirstOrDefaultAsync();

            return View();
		}

		#endregion

		#region post

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginVM loginVM)
		{

			AppUser appUser = await _userManager.FindByNameAsync(loginVM.Username);
			if (appUser == null)
			{
				ModelState.AddModelError("", "İstifadəçi adı və ya şifrə yanlışdır !!!");
				return View();
			}
			if (appUser.IsDeactive)
			{

				ModelState.AddModelError("", "Sizin Akkaunt bloklanıb!!!");
				return View();
			}
			Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signManager.PasswordSignInAsync(appUser, loginVM.Password, loginVM.IsRemember, true);
			if (signInResult.IsLockedOut)
			{

				ModelState.AddModelError("", "Sizin Akkaunt 5 dəqiqəlik bloklanıb!!!");
				return View();
			}
			if (!signInResult.Succeeded)
			{
				ModelState.AddModelError("", "İstifadəçi adı və ya şifrə yanlışdır !!!");
				return View();
			}
			return RedirectToAction("Index", "Home");
		}
        #endregion

        #endregion

        #region Register

        #region get
        public async Task<IActionResult> Register()
        {
            ViewBag.BackPhoto = await _db.BackPhotos.FirstOrDefaultAsync();

            return View();
        }
        #endregion

        #region post
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterVM registerVM)
		{

			AppUser appUser = new AppUser
			{
				Name = registerVM.Name,
				Surname = registerVM.Surname,
				Email = registerVM.Email,
				UserName = registerVM.Username,
			};
			IdentityResult identityResult = await _userManager.CreateAsync(appUser, registerVM.Password);
			if (!identityResult.Succeeded)
			{
				foreach (IdentityError error in identityResult.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
				return View();
			}

			await _userManager.AddToRoleAsync(appUser, Helper.Roles.Admin.ToString());
			await _signManager.SignInAsync(appUser, registerVM.IsRemember);

			return RedirectToAction("Index", "Home");
		}

		#endregion

		#endregion

		#region LogOut
		public async Task<IActionResult> Logout()
		{
			await _signManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
		#endregion

		#region CreateRoles
		//public async Task CreateRoles()
		//{
		//	if (!await _roleManager.RoleExistsAsync(Helper.Roles.Admin.ToString()))
		//	{
		//		await _roleManager.CreateAsync(new IdentityRole { Name = Helper.Roles.Admin.ToString() });

		//	}
		//	if (!await _roleManager.RoleExistsAsync(Helper.Roles.Member.ToString()))
		//	{
		//		await _roleManager.CreateAsync(new IdentityRole { Name = Helper.Roles.Member.ToString() });

		//	}
		//}
		#endregion


	}
}
