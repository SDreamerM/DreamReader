using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using DreamReader.Business.Definitions;
using DreamReader.Database.Entities;
using DreamReader.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace DreamReader.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserManager _userManager;

        private DreamReaderUserManager _dreamReaderUserManager;
        private DreamReaderSignInManager _signInManager;

        public AccountController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public AccountController(DreamReaderUserManager _dreamReaderUserManager, DreamReaderSignInManager signInManager)
        {
            SignInManager = signInManager;
            DreamReaderUserManager = _dreamReaderUserManager;
        }

        public DreamReaderUserManager DreamReaderUserManager
        {
            get
            {
                return _dreamReaderUserManager ?? HttpContext.GetOwinContext().GetUserManager<DreamReaderUserManager>();
            }
            private set
            {
                _dreamReaderUserManager = value;
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
                var result = await DreamReaderUserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, false, false);
                    return JsonSuccess(true);
                }
                AddErrors(result);
            }
            return JsonFailure(GetModelStateError());
        }

        [HttpPost]
        public JsonResult UploadProfileImage()
        {
            var file = Request.Files[0];
            using (var memoryStream = new MemoryStream())
            {
                file.InputStream.CopyTo(memoryStream);
                var base64 = Convert.ToBase64String(memoryStream.ToArray());

                _userManager.UpdateProfileImage(User.Identity.GetUserId(), base64);

                var thumbnailUrl = string.Format(@"data:image/png;base64,{0}", base64);
                return JsonSuccess(thumbnailUrl);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetProfile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return JsonSuccess(new ProfileViewModel
                {
                    IsAuthenticated = User.Identity.IsAuthenticated,
                    ProfileImageUrl = Url.Content("~/Content/Images/no-profile-image.jpg")
                });
            }

            var profile = _userManager.GetProfile(User.Identity.GetUserId());
            var profileViewModel = new ProfileViewModel
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                ProfileImageUrl = profile.Base64ProfileImage == null ? Url.Content("~/Content/Images/no-profile-image.jpg") : string.Format(@"data:image/png;base64,{0}", profile.Base64ProfileImage)
            };
            return JsonSuccess(profileViewModel);
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