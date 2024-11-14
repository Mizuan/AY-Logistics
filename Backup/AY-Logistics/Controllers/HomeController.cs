using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AYLogistics.Attributes;
/*email*/
using AYLogistics.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AYLogistics.Controllers
{
    [AccessDeniedAuthorize(Roles = "Documentation, Admin, Clearance, Account")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Sent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
 #region Async Email
        /* public async Task<ActionResult> Contact(EmailFormModel model)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("muhammadhameen@gmail.com"));  // replace with valid value 
                message.Subject = "Your email subject";
                message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
                message.IsBodyHtml = true;

                try
                {
                    using (var smtp = new SmtpClient())
                    {
                        await smtp.SendMailAsync(message);
                       // return RedirectToAction("Sent");
                        ViewBag.Status = "ok";
                    }
                }
                catch (Exception)
                {
                    ViewBag.Status = "Problem while sending email, Please check details.";
                }
            }
            return View(model);
        }*/
 #endregion // not working 
        public ActionResult Contact(EmailFormModel model)
        {
            try
            {
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";

                MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add("muhammadhameen@gmail.com");
              //  mailMessage.From = new MailAddress("7994181@gmail.com");
                mailMessage.Subject = "ASP.NET e-mail test";
                mailMessage.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
                mailMessage.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    /*var credential = new NetworkCredential // this block Moved to Webconfig
                    {
                        UserName = "7994181@gmail.com",
                        Password = "password" 
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;*/
                    smtp.Send(mailMessage);
                    ViewBag.type = "alert-success";
                    return View(model).Success("E-mail sent!");
                }
            }
            catch (Exception ex)
            {
                ViewBag.type = "alert-error";
                return View(model).Error("Could not send the e-mail - error: " + ex.Message);
            }
        }
    }
}

