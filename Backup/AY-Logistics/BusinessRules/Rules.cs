using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using AYLogistics.Models;
using System.Data.SqlClient;
using System.Web.Security;

namespace AYLogistics.BusinessRules
{
    public class HasLogin : ISpecification<int>
    {
        public bool IsSatisfiedBy(int ACSUserId)
        {
            foreach (ProfileInfo profileInfo in ProfileManager.GetAllProfiles(ProfileAuthenticationOption.All))
            {
                Profile profile = Profile.GetProfile(profileInfo.UserName);
                if (profile.ACSUserId == ACSUserId)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public class ValidFileType : ISpecification<string>
    {
        public bool IsSatisfiedBy(String ext)
        {
            List<String> allowedFileTypes = MySettings.GetAllowedFileTypes();
            foreach (String allowedExt in allowedFileTypes)
            {
                if (allowedExt.Equals(ext, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}