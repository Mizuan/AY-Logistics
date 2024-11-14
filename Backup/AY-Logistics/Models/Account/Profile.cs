using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.ComponentModel.DataAnnotations;

namespace AYLogistics.Models
{
    public class Profile : ProfileBase
    {
        [Display(Name="ACSUser Id")]
        public virtual int ACSUserId
        {
            get
            {
                return Convert.ToInt32(this.GetPropertyValue("ACSUserId"));
            }
            set
            {
                this.SetPropertyValue("ACSUserId", value);
            }
        }

        public static Profile GetProfile(string username)
        {
            return Create(username) as Profile;
        }
    }
}