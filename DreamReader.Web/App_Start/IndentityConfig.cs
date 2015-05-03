using System.Security.Claims;
using System.Threading.Tasks;
using DreamReader.Database;
using DreamReader.Database.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace DreamReader.Web
{
    public class DreamReaderUserManager : UserManager<DreamReaderUser>
    {
        public DreamReaderUserManager(IUserStore<DreamReaderUser> store) : base(store) { }

        public static DreamReaderUserManager Create(IdentityFactoryOptions<DreamReaderUserManager> options, IOwinContext context)
        {
            var manager = new DreamReaderUserManager(new UserStore<DreamReaderUser>(context.Get<DreamReaderContext>()));
            return manager;
        }
    }

    public class DreamReaderSignInManager : SignInManager<DreamReaderUser, string>
    {
        public DreamReaderSignInManager(DreamReaderUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(DreamReaderUser user)
        {
            return user.GenerateUserIdentityAsync((DreamReaderUserManager)UserManager);
        }

        public static DreamReaderSignInManager Create(IdentityFactoryOptions<DreamReaderSignInManager> options, IOwinContext context)
        {
            return new DreamReaderSignInManager(context.GetUserManager<DreamReaderUserManager>(), context.Authentication);
        }
    }
}