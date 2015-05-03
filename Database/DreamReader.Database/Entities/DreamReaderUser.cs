using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DreamReader.Database.Entities
{
    public class DreamReaderUser : IdentityUser
    {
        //public virtual ICollection<Book> Books { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<DreamReaderUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
}
