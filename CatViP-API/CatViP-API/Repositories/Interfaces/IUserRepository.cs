using CatViP_API.DTOs.AuthDTOs;
using CatViP_API.Models;

namespace CatViP_API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> AuthenticateUser(UserLoginRequestDTO userLogin);
        Task<User?> GetUserById(long userId);
        Task<User?> GetSearchUserById(long userId);
        string GetUserRoleName(User user);
        Task<bool> UpdateUserToken(long userId, string JWT, DateTime TokenCreated, DateTime TokenExpires);
        Task<bool> DeleteUserToken(long userId);
        bool CheckIfUsernameExist(string username);
        bool CheckIfEmailExist(string email);
        Task<User?> StoreUser(UserRegisterRequestDTO userRegisterDTO);
        Task<bool> UpdateUserProfile(long userId, EditProfileDTO editProfileDTO);
        Task<bool> ResetUserPassword(string email, string password);
        int GetUserFollowerCount(long UserId);
        int GetUserFollowingCount(long UserId);
        int GetExpertTipsCount(long iUserIdd);
        ICollection<User> SearchByUsenameOrFullName(string name, long authId);
        bool CheckIfIsFollowed(long authId, long id);
        Task<bool> FollowUser(long authId, long userId);
        Task<bool> UnfollowUser(long authId, long userId);
        User? GetActiveCatOwnerOrExpertByUsername(string username);
        List<User> GetOtherActiveCatOwnerAndExpert(long authId);
    }
}
