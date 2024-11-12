using Konar.az.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace Konar.az.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]

	public class MailController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Index(MailRequest mailRequest)
		{
			MimeMessage mimeMessage = new MimeMessage(); 
			MailboxAddress mailboxAddressFrom =
				new MailboxAddress("admin", "musaoglanov1@gmail.com");
			mimeMessage.From.Add(mailboxAddressFrom);

			MailboxAddress mailboxAddressTo=
				new MailboxAddress("User", mailRequest.ReceiverMail);
			mimeMessage.To.Add(mailboxAddressTo);

			var bodyBuilder=new BodyBuilder();
			bodyBuilder.TextBody = mailRequest.Body;
			mimeMessage.Body=bodyBuilder.ToMessageBody();

			mimeMessage.Subject=mailRequest.Subject;

			SmtpClient client = new SmtpClient();
			client.Connect("smtp.gmail.com", 587, false);
			client.Authenticate("musaoglanov1@gmail.com", "ddfu ypii ffas bapp");
			client.Send(mimeMessage);
			client.Disconnect(true);
			return View();
		}
	}
}
//musaoglanov27@gmail.com
