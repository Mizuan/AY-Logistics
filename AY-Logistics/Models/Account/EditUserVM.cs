using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.SqlClient;
using System.Data;
using MyDBAccess;

namespace AYLogistics.Models
{
    public class EditUserVM
    {
        [Required(ErrorMessage="*")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage="*")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        //[Required(ErrorMessage="*")]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        //public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }

        [Required(ErrorMessage="*")]
        [Display(Name = "ACS UserId")]
        public int ACSUserId { get; set; }

        public List<String> Roles { get; set; }

        public EditUserVM() {
            //default construcotor
        }

        public EditUserVM(string username)
        {
            foreach (MembershipUser user in Membership.GetAllUsers())
            {
                Profile profile = Profile.GetProfile(user.UserName);
            }
            
        }

        public static Dictionary<object, object> GetEmploy(int id) {

            string sql = @"	SELECT
                            EM.Id,
	                        EM.EmpId AS UserId,
	                        EM.NIC,
	                        EM.FirstName + ISNULL(EM.MiddleName,'') + ISNULL(EM.LastName,'') as Name	
	                        FROM ACSUser SU
	                        INNER JOIN Employee EM ON EM.Id = SU.ACSUserId
	                        WHERE SU.Id=@Id";

            List<SqlParameter> list  = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Id",DbType.Int32);
            param.Value = id;
            list.Add(param);

            SqlDataReader reader = DBAccess.FetchResult(sql, list, ConnectionString.DEFAULT);
            Dictionary<Object, Object> dictionary = null;
            if (reader.Read())
            {
                dictionary = new Dictionary<object, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    dictionary.Add(reader.GetName(i), reader.GetValue(i));
                }
            }
            reader.Close();
            return dictionary;
        }      

        public static Dictionary<object, object> getUsers() {

            List<Dictionary<object, object>> UserList = new List<Dictionary<object, object>>();

            foreach (MembershipUser user in Membership.GetAllUsers())
            {
                Dictionary<Object, Object> User = new Dictionary<object, object>();
                Profile profile = Profile.GetProfile(user.UserName);
                Dictionary<object,object> ACSUser = GetEmploy(profile.ACSUserId);
                User.Add("Approve",user.IsApproved);
                User.Add("Username",user.UserName);
                User.Add("ACSUserId", profile.ACSUserId);
                User.Add("UserId", ACSUser == null ? "-" : ACSUser["UserId"]);
                User.Add("Name", ACSUser == null ? "-" : ACSUser["Name"]);
                User.Add("Id", ACSUser == null ? "-" : ACSUser["Id"]);
                User.Add("NIC", ACSUser == null ? "-" : ACSUser["NIC"]);
                UserList.Add(User);
            }

            Dictionary<object, object> dataTable = new Dictionary<object, object>();
            dataTable.Add("aaData", UserList);
            return dataTable;
        }
    }

    public class EditUserPasswordVM{
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
        [Display(Name = "Confirm Password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage="*")]
        [Display(Name = "ACS User Id")]
        public int ACSUserId { get; set; }

        public List<String> Roles { get; set; }

        public EditUserPasswordVM()
        {
            //default construcotor
        }

        public EditUserPasswordVM(string username)
        {
            foreach (MembershipUser user in Membership.GetAllUsers())
            {
                Profile profile = Profile.GetProfile(user.UserName);
            }
            
        }
    
    }
}