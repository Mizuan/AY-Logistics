using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AYLogistics.Attributes
{
    public class AccessDeniedAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("~/Account/Logon");
                return;
            }

            if (filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectResult("~/Account/Denied");
            }
        }
    }
    //public class AccessDeniedAuthorizeAttribute : AuthorizeAttribute
    //{
    //    public string AccessDeniedViewName { get; set; }

    //    public override void OnAuthorization(AuthorizationContext filterContext)
    //    {
    //        base.OnAuthorization(filterContext);

    //        if (filterContext.HttpContext.User.Identity.IsAuthenticated &&
    //            filterContext.Result is HttpUnauthorizedResult)
    //        {
    //            if (string.IsNullOrWhiteSpace(AccessDeniedViewName))
    //                AccessDeniedViewName = "~/Account/AccessDenied";

    //            filterContext.Result = new RedirectResult(AccessDeniedViewName);
    //        }
    //    }
    //}
}