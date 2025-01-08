using Konar.az.Models;
using Konar.az.ViewModels;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace Konar.az.Controllers
{
    //[AllowAnonymous]
    public class PasswordChangeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public PasswordChangeController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM forgetPasswordVM)

        {
            if (!ModelState.IsValid)
            {
                return View(forgetPasswordVM); // Düzgün şəkildə dolmadığı halda eyni səhifəyə yönləndir
            }
            var user = await _userManager.FindByEmailAsync(forgetPasswordVM.Mail);
            if (user == null)
            {
                ModelState.AddModelError("Mail", "Bu e-poçt ünvanına uyğun istifadəçi tapılmadı.");
                return View(forgetPasswordVM);
            }
            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var passwordResetTokenLink = Url.Action("ResetPassword", "PasswordChange",
                new
                {
                    userId = user.Id,
                    token = passwordResetToken
                }, HttpContext.Request.Scheme);

            //Mail Request

            MimeMessage mimeMessage = new MimeMessage();
            MailboxAddress mailboxAddressFrom =
                new MailboxAddress("admin", "musaoglanov1@gmail.com");
            mimeMessage.From.Add(mailboxAddressFrom);

            MailboxAddress mailboxAddressTo =
                new MailboxAddress("user", forgetPasswordVM.Mail);
            mimeMessage.To.Add(mailboxAddressTo);

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = passwordResetTokenLink;
            mimeMessage.Body = bodyBuilder.ToMessageBody();
            mimeMessage.Subject = "Şifrə dəyişiklik tələbi ";

            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com", 587, false);
            client.Authenticate("musaoglanov1@gmail.com", "ddfu ypii ffas bapp");
            client.Send(mimeMessage);
            client.Disconnect(true);



            return View();
        }


        public IActionResult ResetPassword(string userid, string token)
        {
            TempData["userid"] = userid;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            var userid = TempData["userid"];
            var token = TempData["token"];
            if (userid == null || token == null)
            {
                return NotFound();
            }
            var user=await _userManager.FindByIdAsync(userid.ToString());
            var result=await _userManager.ResetPasswordAsync(user,token.ToString(),resetPasswordVM.Password);
            if(result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }




    }
}
