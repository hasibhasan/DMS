using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using DMS.Models.ViewModel;
using DataModel.Models.EntityManager;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.AspNet.Identity.Owin;
using reCAPTCHA.MVC;
using DataModel.Models.DB;
using DataModel.Models.ViewModel;


namespace DMS.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }
        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }
        public UserManager<ApplicationUser> UserManager { get; private set; }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                    {
                        ViewBag.errorMessage = "You must have a confirmed email to log on.";
                        return View();
                    }
                    else
                    {
                        await SignInAsync(user, model.RememberMe);
                        return RedirectToLocal(returnUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
            return View(model);
        }

        //
        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(string emailTxtBox)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(emailTxtBox);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    ViewBag.Message = "Please insert a valid email!!!";
                    return View("Info");
                }
                var provider = new DpapiDataProtectionProvider("DMS");
                UserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(provider.Create("ForgotPasswordConfirmation"));
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                string body = "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>";
                EmailManager emm = new EmailManager();
                bool isMailSend = await emm.EmailSend(body, emailTxtBox);
                ViewBag.Message = "Check your email and confirm your password reset request";
                return View("Info");
            }
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(string userId = null, string code = null)
        {
            if (userId == null || code == null)
            {
                ViewBag.Message = "Sorry..Invalid Request!!";
                return View("info");
            }

            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.Message = "Sorry..Invalid Request!!";
                return View("info");
            }
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return RedirectToAction("ResetConfirmation", "Account");
                }
                var provider = new DpapiDataProtectionProvider("DMS");
                UserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(
                    provider.Create("ForgotPasswordConfirmation"));
                var result = await UserManager.ResetPasswordAsync(model.UserId, model.Code, model.Password);
                if (result.Succeeded)
                {
                    ViewBag.Message = "Your Password has been reset :)";
                    return View("info");
                }
                else
                {
                    ViewBag.Message = "Session Expired.Please try a new request.";
                    return View("info");
                }
                return View(model);
            }
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {

            //Create Basic Two Roles when firsts setup
            var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var rolesToBeCreated = new List<string> { "Administrator", "User" };
            foreach (var roles in rolesToBeCreated)
            {
                if (!roleManager.RoleExists(roles))
                {
                    var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                    role.Name = roles;
                    roleManager.Create(role);
                    ViewBag.Role = "Quick Installation: First Time Role Created";
                }

            }
            CommonManager cmn = new CommonManager();
            ViewBag.CountryList = cmn.GetCountryList();
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [CaptchaValidator(
           PrivateKey = "6LdixBoUAAAAAA0XRMcBi2JBOvLqVQZyDwI_9Nyl",
           ErrorMessage = "Invalid input captcha.",
           RequiredMessage = "The captcha field is required.")]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {


                var user = new ApplicationUser() { UserName = model.UserName, Address = model.Address, Email = model.Email, Phone = model.Phone, NID = model.NID, Gender = model.Gender, FirstName = model.FirstName, LastName = model.LastName, DateOfBirth = model.DateOfBirth };
                try
                {

                    if (UserManager.FindByName(model.UserName) == null)
                    {
                        if (UserManager.FindByEmail(model.Email) == null)
                        {
                            var result = await UserManager.CreateAsync(user, model.Password);
                            UserManager.AddToRole(user.Id, "User");
                            if (result.Succeeded)
                            {
                                //await SignInAsync(user, isPersistent: false);                               
                                //return RedirectToAction("Login", "Account");
                                var provider = new DpapiDataProtectionProvider("DMS");


                                UserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(
                                    provider.Create("EmailConfirmation"));

                                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                                var callbackUrl = Url.Action("ConfirmEmail", "Account",
                                   new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                                //await UserManager.SendEmailAsync(user.Id,
                                //   "Confirm your account", "Please confirm your account by clicking <a href=\""
                                //   + callbackUrl + "\">here</a>");

                                string body = "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>";

                                EmailManager emm = new EmailManager();
                                bool isMailSend = await emm.EmailSend(body, user.Email);
                                ViewBag.Message = "Check your email and confirm your account, you must be confirmed "
                                                + "before you can log in.";
                                return View("Info");
                                //return RedirectToAction("Index", "Home");
                            }

                            else
                            {
                                AddErrors(result);
                            }
                        }
                        else
                        {
                            ViewBag.validationerror = "Hmm...Email already taken!";
                        }
                    }
                    else
                    {
                        ViewBag.validationerror = "Hmm...Wrong user id!";
                    }
                }
                catch
                {
                    ViewBag.validationerror = "Hmm...Something went wrong! Please fill the form properly!";
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            try
            {
                var provider = new DpapiDataProtectionProvider("DMS");
                UserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(
                    provider.Create("EmailConfirmation"));
                if (userId == null || code == null)
                {
                    return View("Error");
                }
                var result = await UserManager.ConfirmEmailAsync(userId, code);
                if (result.Succeeded)
                {
                    return View("ConfirmEmail");
                }
                AddErrors(result);
                return View();
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: /Account/Profile
        public ActionResult Profile()
        {            
            UserManagementManager umm = new UserManagementManager();
            UserManagementViewModel uvv = umm.SelectUserDetails(User.Identity.GetUserId());
            if (uvv == null)
            {
                return HttpNotFound();
            }
            string sqlFormattedDate = uvv.DateOfBirth.HasValue ? uvv.DateOfBirth.Value.ToString("yyyy-MM-dd") : "";
            ViewBag.FormattedDate = sqlFormattedDate;
            return View(uvv);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]        
        public ActionResult Profile([Bind(Include = "Id,UserName,FirstName,LastName,Gender,Email,Address,Phone,NID,DateOfBirth")] UserManagementViewModel umViewModel)
        {
            if (ModelState.IsValid)
            {
                UserManagementManager um = new UserManagementManager();
                um.UpdateUserAccount(umViewModel);
                return RedirectToAction("Profile");
            }
            return View("~/Views/Admin/Role/Edit.cshtml", umViewModel);
        }



        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}