using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DreamReader.Database.Entities;
using DreamReader.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace DreamReader.Web.Controllers
{
    public class AccountController : BaseController
    {
        private DreamReaderUserManager _userManager;
        private DreamReaderSignInManager _signInManager;

        public AccountController() { }

        public AccountController(DreamReaderUserManager userManager, DreamReaderSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public DreamReaderUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<DreamReaderUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public DreamReaderSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<DreamReaderSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SignIn(SignInViewModel model)
        {
            if (!ModelState.IsValid)
                return JsonFailure(GetModelStateError());

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result == SignInStatus.Success)
                return JsonSuccess(true);

            ModelState.AddModelError("", "Invalid login attempt.");
            return JsonFailure(GetModelStateError());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new DreamReaderUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, false, false);
                    return JsonSuccess(true);
                }
                AddErrors(result);
            }
            return JsonFailure("Get ModelState Errors");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return JsonSuccess(true);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private string GetModelStateError()
        {
            var result = string.Empty;
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    result += error.ErrorMessage;
                }
            }
            return result;
        }
    }
}