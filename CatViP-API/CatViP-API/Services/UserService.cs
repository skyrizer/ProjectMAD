using AutoMapper;
using CatViP_API.DTOs.AuthDTOs;
using CatViP_API.DTOs.CatDTOs;
using CatViP_API.DTOs.UserDTOs;
using CatViP_API.Models;
using CatViP_API.Repositories;
using CatViP_API.Repositories.Interfaces;
using CatViP_API.Services.Interfaces;

namespace CatViP_API.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
            this._configuration = configuration;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseResult<SerachUserInfoDTO>> GetSearchUserInfo(long authId, long userId)
        {
            var res = new ResponseResult<SerachUserInfoDTO>();

            var user = await _userRepository.GetSearchUserById(userId);

            if (user == null)
            {
                res.IsSuccessful = false;
                res.ErrorMessage = "Invalid User Id.";
                return res;
            }

            var userDTO = _mapper.Map<SerachUserInfoDTO>(user!);
            userDTO.IsExpert = (user!.RoleId == 3);
            userDTO.Following = _userRepository.GetUserFollowingCount(user!.Id);
            userDTO.Follwer = _userRepository.GetUserFollowerCount(user!.Id);
            userDTO.IsFollowed = _userRepository.CheckIfIsFollowed(authId, user!.Id);

            if (userDTO.IsExpert)
            {
                userDTO.ExpertTips = _userRepository.GetExpertTipsCount(user.Id);
            }

            res.Result = userDTO;

            return res;
        }

        public async Task<ResponseResult<UserInfoDTO>> GetUserInfoById(long id)
        {
            var res = new ResponseResult<UserInfoDTO>();

            var user = await _userRepository.GetUserById(id);

            if (user == null)
            {
                res.IsSuccessful = false;
                res.ErrorMessage = "Invalid User Id.";
                return res;
            }
            
            var userDTO = _mapper.Map<UserInfoDTO>(user!);
            userDTO.IsExpert = (user!.RoleId == 3);
            userDTO.Following = _userRepository.GetUserFollowingCount(user!.Id);
            userDTO.Follwer = _userRepository.GetUserFollowerCount(user!.Id);

            if (userDTO.IsExpert)
            {
                userDTO.ExpertTips = _userRepository.GetExpertTipsCount(user.Id);
            }

            res.Result = userDTO;

            return res;
        }

        public ICollection<SearchUserDTO> SearchByUsenameOrFullName(string name, long authId)
        {
            return _mapper.Map<ICollection<SearchUserDTO>>(_userRepository.SearchByUsenameOrFullName(name, authId));
        }

        public async Task<ResponseResult> FollowUser(long authId, long userId)
        {
            var res = new ResponseResult();

            res.IsSuccessful = await _userRepository.FollowUser(authId, userId);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "Fail to follow user.";
            }

            return res;
        }

        public async Task<ResponseResult> UnfollowUser(long authId, long userId)
        {
            var res = new ResponseResult();

            res.IsSuccessful = await _userRepository.UnfollowUser(authId, userId);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "Fail to unfollow user.";
            }

            return res;
        }
    }
}
