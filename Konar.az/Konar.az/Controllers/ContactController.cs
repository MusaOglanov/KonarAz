using Konar.az.DAL;
using Konar.az.Models;
using Konar.az.ViewModels;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Web.Helpers;

namespace Konar.az.Controllers
{
	public class ContactController : Controller
	{
		private readonly AppDbContext _db;

		public ContactController(AppDbContext db)
		{
			_db = db;
		}

		public async Task<IActionResult> Index()
		{
            ViewBag.BackPhoto = await _db.BackPhotos.FirstOrDefaultAsync();

            ContactVM contactVM = new ContactVM
			{
				ContactBase = await _db.Contacts.FirstOrDefaultAsync(),
				ContactMessages = new ContactMessage()

			};
			return View(contactVM);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Index(ContactVM model)
		{
			if (!ModelState.IsValid)
			{
				// Əgər forma valid deyil, Contact məlumatını yenidən yükləyirik.
				model.ContactBase = await _db.Contacts.FirstOrDefaultAsync();
				return View(model);
			}

			// E-poçt göndərmə
			var mailMessage = new MimeMessage();
			mailMessage.From.Add(new MailboxAddress("Sayt Adı", "musaoglanov1@gmail.com"));
			mailMessage.To.Add(new MailboxAddress("Admin", "musaoglanov1@gmail.com"));
			mailMessage.Subject = $"Yeni əlaqə mesajı: {model.ContactMessages.Subject}";

			mailMessage.Body = new TextPart("plain")
			{
				Text = $"Ad: {model.ContactMessages.Name}\nTelefon: {model.ContactMessages.Phone}\nEmail: {model.ContactMessages.Email}\nMövzu: {model.ContactMessages.Subject}\nMesaj: {model.ContactMessages.Message}"
			};

			using (var smtpClient = new SmtpClient())
			{
				smtpClient.Connect("smtp.gmail.com", 587, false);
				smtpClient.Authenticate("musaoglanov1@gmail.com", "ddfu ypii ffas bapp");
				smtpClient.Send(mailMessage);
				smtpClient.Disconnect(true);
			}

			// Məlumatı verilənlər bazasında saxlayırıq
			_db.ContactMessages.Add(model.ContactMessages);
			await _db.SaveChangesAsync();

			ViewBag.Success = "Mesaj uğurla göndərildi!";

			// Contact məlumatını yenidən yükləyirik
			model.ContactBase = await _db.Contacts.FirstOrDefaultAsync();
			return View(model);
		}


		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> Index(string name, string phone, string email, string subject, string message)
		//{
		//	// E-poçt yaratmaq
		//	var mailMessage = new MimeMessage();
		//	mailMessage.From.Add(new MailboxAddress("Sayt Adı", "musaoglanov1@gmail.com"));
		//	mailMessage.To.Add(new MailboxAddress("Admin", "musaoglanov1@gmail.com"));
		//	mailMessage.Subject = $"Yeni əlaqə mesajı: {subject}";

		//	mailMessage.Body = new TextPart("plain")
		//	{
		//		Text = $"Ad: {name}\nTelefon: {phone}\nEmail: {email}\nMövzu: {subject}\nMesaj: {message}"
		//	};

		//	// SMTP ilə göndərmək
		//	using (var smtpClient = new SmtpClient())
		//	{
		//		smtpClient.Connect("smtp.gmail.com", 587, false);
		//		smtpClient.Authenticate("musaoglanov1@gmail.com", "ddfu ypii ffas bapp");
		//		smtpClient.Send(mailMessage);
		//		smtpClient.Disconnect(true);
		//	}

		//	// Məlumatı verilənlər bazasında saxlamaq
		//	var contactMessage = new ContactMessage
		//	{
		//		Name = name,
		//		Phone = phone,
		//		Email = email,
		//		Subject = subject,
		//		Message = message
		//	};

		//	_db.ContactMessages.Add(contactMessage);
		//	await _db.SaveChangesAsync();

		//	ViewBag.Success = "Mesaj uğurla göndərildi!";
		//	return View();
		//}
	}
}
