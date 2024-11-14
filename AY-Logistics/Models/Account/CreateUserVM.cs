using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using MyDBAccess;

namespace AYLogistics.Models
{
    public class CreateUserVM
    {
        [Required(ErrorMessage="*")]
        [Display(Name = "Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage="*")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage="*")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage="*")]
        [Display(Name = "UserId")]
        public int ACSUserId { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name="Roles")]
        public List<String> Roles { get; set; }



        public Dictionary<object, object> GetEmployeeEmail(int ACSUId)
        {
            string sql = @"SELECT   SU.Id,
		                            E.NIC,
		                            E.EmpId,
		                            E.FirstName,
		                            E.MiddleName,
		                            E.LastName,
		                            E.Email
	                            FROM ACSUser SU
	                            INNER JOIN Employee E ON E.Id = SU.ACSUserId AND ACSUserType=2
	                            WHERE E.EmployeeStatusId =1 AND SU.Id = @ACSUId";

            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@ACSUId", DbType.Int32);
            param.Value = ACSUId;
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            Dictionary<object, object> Dictionary = new Dictionary<object, object>();

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Dictionary.Add(reader.GetName(i), reader.GetValue(i));
                }
                reader.Close();
                return Dictionary;
            }
            reader.Close();
            return null;
        }
    }
}