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
        public IActionResult About()
        {
            return View();
        }
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
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

                // Receiver (Your Email)
                mail.To.Add("Harshitdelhisix@gmail.com");

                // Subject
                mail.Subject = $"📩 New Contact Form: {model.Subject}";

                // HTML Email
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

<!-- Header -->
<tr>
<td style='background:linear-gradient(135deg,#0d6efd,#6610f2);
padding:35px;
text-align:center;
color:#ffffff;'>

<h1 style='margin:0;font-size:30px;font-weight:bold;'>
📩 New Portfolio Inquiry
</h1>

<p style='margin-top:10px;font-size:16px;'>
You have received a new message from your website.
</p>

</td>
</tr>

<!-- Body -->
<tr>
<td style='padding:35px;'>

<table width='100%' cellpadding='12' cellspacing='0'
style='border-collapse:collapse;
font-size:15px;'>

<tr>
<td style='width:170px;
background:#f8f9fa;
font-weight:bold;
color:#0d6efd;
border:1px solid #dee2e6;'>

👤 Name

</td>

<td style='border:1px solid #dee2e6;'>

{model.Name}

</td>
</tr>

<tr>
<td style='background:#f8f9fa;
font-weight:bold;
color:#0d6efd;
border:1px solid #dee2e6;'>

📧 Email

</td>

<td style='border:1px solid #dee2e6;'>

<a href='mailto:{model.Email}'
style='color:#0d6efd;text-decoration:none;'>

{model.Email}

</a>

</td>
</tr>

<tr>
<td style='background:#f8f9fa;
font-weight:bold;
color:#0d6efd;
border:1px solid #dee2e6;'>

📝 Subject

</td>

<td style='border:1px solid #dee2e6;'>

{model.Subject}

</td>
</tr>

</table>

<h2 style='margin-top:35px;
color:#0d6efd;
border-bottom:2px solid #0d6efd;
padding-bottom:8px;'>

💬 Message

</h2>

<div style='background:#f8f9fa;
border-left:5px solid #0d6efd;
padding:20px;
border-radius:8px;
font-size:15px;
line-height:1.8;
color:#444;'>

{model.Message.Replace(Environment.NewLine, "<br/>")}

</div>

</td>
</tr>

<!-- Footer -->
<tr>
<td style='background:#212529;
padding:25px;
text-align:center;
color:#ffffff;'>

<p style='margin:0;font-size:15px;'>
This email was automatically generated from your
<strong>Portfolio Contact Form</strong>.
</p>

<p style='margin-top:10px;font-size:13px;color:#adb5bd;'>

© {DateTime.Now.Year} Harshit Portfolio. All Rights Reserved.

</p>

</td>
</tr>

</table>

</td>
</tr>
</table>

</body>
</html>";

                // SMTP Configuration
                SmtpClient smtp = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false
                };

                // Send Email
                smtp.Send(mail);

                ViewBag.Success = "Your message has been sent successfully.";

                ModelState.Clear();

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(model);
            }
        }
        public IActionResult Services()
        {
            return View();
        }
        public IActionResult ServiceDetails(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        public IActionResult Portfolio()
        {
            return View();
        }
       
        public IActionResult PortfolioDetails()
        {
            return View();
        }
        public IActionResult Resume()
        {
            return View();
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
