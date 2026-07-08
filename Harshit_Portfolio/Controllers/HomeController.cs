using Harshit_Portfolio.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace Harshit_Portfolio.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
       
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill all required fields.";
                return RedirectToAction("Index");
            }

            try
            {
                string host = _configuration["Email:SmtpHost"];
                int port = Convert.ToInt32(_configuration["Email:SmtpPort"]);
                string fromEmail = _configuration["Email:FromEmail"];
                string fromName = _configuration["Email:FromName"];
                string username = _configuration["Email:Username"];
                string password = _configuration["Email:Password"];

                MailMessage mail = new MailMessage();

                // Sender
                mail.From = new MailAddress(fromEmail, fromName);

                // Receiver
                mail.To.Add("harshitdelhisix@gmail.com");

                // Reply To (User Email)
                mail.ReplyToList.Add(new MailAddress(model.Email, model.Name));

                // Subject
                mail.Subject = $"📩 New Contact Form: {model.Subject}";

                mail.IsBodyHtml = true;

                mail.Body = $@"
<!DOCTYPE html>
<html>
<head>
<meta charset='UTF-8'>
<title>Portfolio Contact</title>
</head>

<body style='margin:0;padding:40px;background:#f3f5f7;font-family:Segoe UI,Arial,sans-serif;'>

<table width='100%' cellpadding='0' cellspacing='0'>
<tr>
<td align='center'>

<table width='700' cellpadding='0' cellspacing='0'
style='background:#ffffff;
border-radius:12px;
overflow:hidden;
box-shadow:0 10px 30px rgba(0,0,0,.15);'>

<tr>
<td style='background:#0d6efd;padding:35px;text-align:center;color:#fff;'>

<h1 style='margin:0;'>📩 New Portfolio Inquiry</h1>

<p>You have received a new message from your website.</p>

</td>
</tr>

<tr>
<td style='padding:35px;'>

<table width='100%' cellpadding='12' cellspacing='0' style='border-collapse:collapse;'>

<tr>
<td style='font-weight:bold;background:#f8f9fa;'>Name</td>
<td>{model.Name}</td>
</tr>

<tr>
<td style='font-weight:bold;background:#f8f9fa;'>Email</td>
<td>{model.Email}</td>
</tr>

<tr>
<td style='font-weight:bold;background:#f8f9fa;'>Subject</td>
<td>{model.Subject}</td>
</tr>

</table>

<h3 style='margin-top:30px;'>Message</h3>

<div style='background:#f8f9fa;padding:20px;border-left:4px solid #0d6efd;'>

{model.Message.Replace(Environment.NewLine, "<br/>")}

</div>

</td>
</tr>

<tr>
<td style='background:#212529;color:#fff;text-align:center;padding:20px;'>

© @DateTime.Now.Year Harshit Portfolio

</td>
</tr>

</table>

</td>
</tr>
</table>

</body>
</html>";

                SmtpClient smtp = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false
                };

                smtp.Send(mail);

                TempData["Success"] = "✅ Your message has been sent successfully.";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction("Index");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
