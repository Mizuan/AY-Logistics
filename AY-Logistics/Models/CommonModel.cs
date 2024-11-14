using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using MyDBAccess;
/*email*/
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AYLogistics.Models
{
    public class GenderModel{
        [Required(ErrorMessage = "*")]
        public int GenderId { get; set; }
        [Display(Name = "Gender")]
        public string GenderName { get; set; }

        public static IQueryable<GenderModel> GetGenders()
        {
            string sql = "SELECT Id,Name FROM Gender;";
            List<SqlParameter> spList = new List<SqlParameter>();
            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            List<GenderModel> list = new List<GenderModel>();
            while (reader.Read())
            {
                GenderModel Gender = new GenderModel { GenderId = (int)reader[0], GenderName = (string)reader[1] };
                list.Add(Gender);
            }
            reader.Close();
            return list.AsQueryable<GenderModel>();
        }
    }

    public class StatusVM
    {
        [Display(Name = "Status")]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> DbStatusList { get; set; }

        public StatusVM()
        {
            //blank constructor
        }

        public static IQueryable<StatusVM> GetStatusList(List<int> StatusList = null)
        {
            List<StatusVM> list = new List<StatusVM>();
            List<SqlParameter> spList = new List<SqlParameter>();

            string sql = "SELECT Id,Name FROM BLStatus ";

            if (StatusList != null)
            {
                sql += " WHERE [Id] IN (%StatusList%)";
                sql = sql.Replace("%StatusList%", String.Join(",", StatusList.Select(i => i.ToString()).ToArray()));
            }

            SqlDataReader reader = DBAccess.FetchResult(sql, spList, ConnectionString.DEFAULT);
            StatusVM status;
            while (reader.Read())
            {
                //status = new StatusVM { Id = 1, Name = "Name", DbStatusList = new List<int> { 1 } };
                status = new StatusVM { Id = Convert.ToInt32(reader[0]), Name = Convert.ToString(reader[1]), DbStatusList = new List<int> { Convert.ToInt32(reader[0]) } };
                list.Add(status);
            }
            reader.Close();
            return list.AsQueryable();
        }

    }

    public class EmailFormModel
    {
        [Required, Display(Name = "Your name")]
        public string FromName { get; set; }
        [Required, Display(Name = "Your email"), EmailAddress]
        public string FromEmail { get; set; }
        [Required]
        public string Message { get; set; }

        public static string SendEmail(string Template, string ToEmail, string Subject)
        {
            try
            {
               // var body = "<p>Email From: {0} ({1})</p><p>Automatically generated email from Asia Forwarding Logistics Software:</p><p>{2}</p>";
                var body = "<p>{2}</p>";
                MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add(ToEmail);
              //  mailMessage.From = new MailAddress("7994181@gmail.com");
                mailMessage.Subject = Subject;
                mailMessage.Body = string.Format(body, "ASIA FORWARDING PVT.LTD", "docs@theasiaforwarding.com", Template);
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
                    return "True";
                }
            }
            catch (Exception ex)
            {
                return "Could not send the e-mail - error: " + ex.Message;
            }
        }

        public static string SendEmailwithAttachment(string Template, string ToEmail, string Subject, MailMessage mailMessage)
        {
            try
            {
                //var body = "<p>Email From: {0} ({1})</p><p>Automatically generated email from Asia Forwarding Logistics Software:</p><p>{2}</p>";
                var body = "<p>{2}</p>";
                mailMessage.To.Add(ToEmail);
                mailMessage.Subject = Subject;
                mailMessage.Body = string.Format(body, "ASIA FORWARDING PVT.LTD", "docs@theasiaforwarding.com", Template);
                mailMessage.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    smtp.Send(mailMessage);
                    return "True";
                }
            }
            catch (Exception ex)
            {
                return "Could not send the e-mail - error: " + ex.Message;
            }
        }
    }



}