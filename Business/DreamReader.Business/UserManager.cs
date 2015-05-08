using System;
using System.Linq;
using DreamReader.Business.Definitions;
using DreamReader.Business.DTOs;
using DreamReader.Database;

namespace DreamReader.Business
{
    public class UserManager : IUserManager
    {
        public ProfileDto GetProfile(string userId)
        {
            using (var dbContext = new DreamReaderContext())
            {
                var user = dbContext.Users.SingleOrDefault(x => x.Id == userId);
                if (user == null)
                    throw new Exception(string.Format("User#{0} not found in the system", userId));

                var profile = new ProfileDto {Base64ProfileImage = user.Base64ProfileImage};
                return profile;
            }
        }

        public void UpdateProfileImage(string userId, string base64ProfileImage)
        {
            using (var dbContext = new DreamReaderContext())
            {
                var user = dbContext.Users.SingleOrDefault(x => x.Id == userId);
                if (user == null)
                    throw new Exception(string.Format("User#{0} not found in the system", userId));

                user.Base64ProfileImage = base64ProfileImage;

                dbContext.SaveChanges();
            }
        }
    }
}
