using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using AYLogistics.Models;
using AYLogistics.Attributes;
using AYLogistics.BusinessRules;

namespace AYLogistics.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Denied() //Access denied page
        {
            return View();
        }
        //
        // GET: /Account/LogOn
        [AllowAnonymous]
        public ActionResult LogOn()
        {
            return View();
        }

        public ActionResult GetEmployeeEmail(int ACSUId)
        {
            CreateUserVM CU = new CreateUserVM();
            if (HttpContext.Request.IsAjaxRequest())
                return Json(CU.GetEmployeeEmail(ACSUId), JsonRequestBehavior.AllowGet);

            return Json(CU.GetEmployeeEmail(ACSUId), JsonRequestBehavior.AllowGet);
        }
        //
        // POST: /Account/LogOn
        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    // Load employee id to session
                    Session["ACSUserId"] = Profile.GetProfile(model.UserName).ACSUserId;
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                       // return RedirectToAction("Index", "Student");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register
        [AccessDeniedAuthorize(Roles = "Admin")]
        public ActionResult CreateUser()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult CreateSA(int ACSUserId)
        {
            if (Membership.GetAllUsers().Count < 1)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser("sa", "123456", "", null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    // add roles
                    foreach (string roleName in Roles.GetAllRoles())
                    {
                        if (Roles.RoleExists(roleName))
                        {
                            Roles.AddUserToRole("sa", roleName);
                        }
                    }
                    //FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    Profile profile = Profile.GetProfile("sa");
                    profile.Initialize("sa", true);
                    profile.ACSUserId = ACSUserId;
                    profile.Save();

                    Response.Write("Supper user has been created! user name: sa, password: 123456, click Url and press ENTER key to goto Logon screen!");
                    Response.End();
                }
                else
                {
                    Response.Write("SA could not be created!");
                    Response.End();
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AccessDeniedAuthorize(Roles = "Admin")]
        public ActionResult CreateUser(CreateUserVM model)
        {
            Dictionary<String, String> dictionary = new Dictionary<string, string>();

            if (ModelState.IsValid)
            {
                HasLogin hasLogin = new HasLogin();

                if (hasLogin.IsSatisfiedBy(model.ACSUserId))
                {
                    ViewBag.type = "alert-error";// this ViewBagType is using to display notification message for My Custom CSS only
                    return View().Error("There is an username for this person, cannot create new one");
                }
                else
                {

                    // Attempt to register the user
                    MembershipCreateStatus createStatus;
                    Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                    if (createStatus == MembershipCreateStatus.Success)
                    {
                        // add roles
                        foreach (string roleName in model.Roles)
                        {
                            if (Roles.RoleExists(roleName))
                            {
                                Roles.AddUserToRole(model.UserName, roleName);
                            }
                        }
                        //FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                        Profile profile = Profile.GetProfile(model.UserName);
                        profile.Initialize(model.UserName, true);
                        profile.ACSUserId = model.ACSUserId;
                        profile.Save();

                        ViewBag.type = "alert-success";// this ViewBagType is using to display notification message for My Custom CSS only
                        return View().Success("User has been created successfully!");
                    }
                    else
                    {
                        ViewBag.type = "alert-error";// this ViewBagType is using to display notification message for My Custom CSS only
                        return View().Error("cannot created user");
                    }
                }
            }
            else
            {
                string error_message = MySettings.ValidationErrorMessage(ModelState, ModelMetadataProviders.Current.GetMetadataForType(null, model.GetType()));
                ViewBag.type = "alert-error";// this ViewBagType is using to display notification message for My Custom CSS only
                return View().Error(error_message);
            }
        }

        [AccessDeniedAuthorize(Roles = "Admin")]
        public JsonResult getUser()
        {
            return Json(EditUserVM.getUsers(), JsonRequestBehavior.AllowGet);
        }

        [AccessDeniedAuthorize(Roles = "Admin")]
        public string ActivateUser(string username)
        {
            MembershipUser user = Membership.GetUser(username);
            if (user.IsApproved == false)
            {
                user.IsApproved = true;
                Membership.UpdateUser(user);
                return "true";
            }
            else
            {
                user.IsApproved = false;
                Membership.UpdateUser(user);
                return "false";
            }

        }

        [AccessDeniedAuthorize(Roles = "Admin")]
        public string DeleteUser(string username)
        {
            Membership.DeleteUser(username);
            return "true";
        }

        [AccessDeniedAuthorize(Roles = "Admin")]
        public ActionResult ViewUser()
        {
            return View();
        }

        [AccessDeniedAuthorize(Roles = "Admin")]
        public ActionResult EditUserPassword(string username = null)
        {
            if (username != null)
            {
                EditUserPasswordVM edit = new EditUserPasswordVM();
                Profile prof = Profile.GetProfile(username);
                edit.Email = Membership.GetUser(username).Email;
                edit.UserName = prof.UserName;
                edit.ACSUserId = prof.ACSUserId;
                return View(edit);
            }
            return View();
        }

        [HttpPost]
        [AccessDeniedAuthorize(Roles = "Admin")]
        public ActionResult EditUserPassword(EditUserPasswordVM model)
        {
            // MembershipCreateStatus createStatus;
            //  Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);
            MembershipUser user = Membership.GetUser(model.UserName);
            //user.ResetPassword();
            //user.UnlockUser();
            user.ChangePassword(user.ResetPassword(), model.Password);
            Membership.UpdateUser(user);
            ViewBag.type = "alert-success";
            return View().Success("password has been changed successfully!");
        }

        [AccessDeniedAuthorize(Roles = "Admin")]
        public ActionResult EditUser(string user)
        {
            EditUserVM edit = new EditUserVM();
            Profile prof = Profile.GetProfile(user);
            edit.Email = Membership.GetUser(user).Email;
            edit.UserName = prof.UserName;
            edit.ACSUserId = prof.ACSUserId;

            return View(edit);
        }

        [HttpPost]
        [AccessDeniedAuthorize(Roles = "Admin")]
        public ActionResult EditUser(EditUserVM model)
        {
            if (Roles.GetRolesForUser(model.UserName).Length != 0)
            {
                Roles.RemoveUserFromRoles(model.UserName, Roles.GetRolesForUser(model.UserName));
            }
            foreach (string roleName in model.Roles)
            {
                if (Roles.RoleExists(roleName))
                {
                    Roles.AddUserToRole(model.UserName, roleName);
                }
            }
            ViewBag.type = "alert-success";// this ViewBagType is using to display notification message for My Custom CSS only
            return View(model).Success("Role has been changed successfully");
        }

        //
        // GET: /Account/Register
        [AccessDeniedAuthorize(Roles = "Admin")]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AccessDeniedAuthorize(Roles = "Admin")]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
