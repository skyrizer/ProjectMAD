using CatViP_API.DTOs.UserDTOs;

namespace CatViP_API.Services.Interfaces
{
    public interface IUserService
    {
        Task<ResponseResult<UserInfoDTO>> GetUserInfoById(long id);
        Task<ResponseResult<SerachUserInfoDTO>> GetSearchUserInfo(long authId, long userId);
        ICollection<SearchUserDTO> SearchByUsenameOrFullName(string name, long authId);
        Task<ResponseResult> FollowUser(long authId, long userId);
        Task<ResponseResult> UnfollowUser(long authId, long userId);
    }
}
