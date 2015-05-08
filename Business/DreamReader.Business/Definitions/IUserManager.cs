using DreamReader.Business.DTOs;

namespace DreamReader.Business.Definitions
{
    public interface IUserManager
    {
        ProfileDto GetProfile(string userId);
        void UpdateProfileImage(string userId, string base64ProfileImage);
    }
}
