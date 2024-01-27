using Azure.Core;
using CatViP_API.Data;
using CatViP_API.DTOs.AuthDTOs;
using CatViP_API.Models;
using CatViP_API.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CatViP_API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CatViPContext _context;

        public UserRepository(CatViPContext context)
        {
            _context = context;
        }

        public async Task<User?> AuthenticateUser(UserLoginRequestDTO userLogin)
        {
            var user = await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Username == userLogin.Username);

            if (user != null)
            {
                var isValidPassword = BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password);

                if (isValidPassword)
                {
                    var roleMap = new Dictionary<long, bool>
                    {
                        {1, false},
                        {2, true},
                        {3, true},
                        {4, false}
                    };

                    if (roleMap.ContainsKey(user.RoleId) && roleMap[user.RoleId] == userLogin.IsMobileLogin)
                    {
                        return user;
                    }
                }
            }

            return null;
        }

        public async Task<bool> DeleteUserToken(long userId)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == userId);

                if (user != null)
                {
                    user.RememberToken = null;
                    user.TokenCreated = null;
                    user.TokenExpires = null;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<User?> GetUserById(long userId)
        {
            return await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == userId);
        }

        public string GetUserRoleName(User user)
        {
            return _context.Roles.FirstOrDefault(x => x.Id == user.RoleId)!.Name;
        }

        public async Task<bool> UpdateUserToken(long userId, string JWT, DateTime TokenCreated, DateTime TokenExpires)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == userId);
                
                if (user != null)
                {
                    user.RememberToken = JWT;
                    user.TokenCreated = TokenCreated;
                    user.TokenExpires = TokenExpires;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheckIfUsernameExist(string username)
        {
            return _context.Users.Any(x => x.Username == username);
        }

        public bool CheckIfEmailExist(string email)
        {
            return _context.Users.Any(x => x.Email == email);
        }
        
        public async Task<User?> StoreUser(UserRegisterRequestDTO userRegisterDTO)
        {
            try
            {
                var user = new User();
                user.Username = userRegisterDTO.Username;
                user.FullName = userRegisterDTO.FullName;
                user.Email = userRegisterDTO.Email;
                user.Password = BCrypt.Net.BCrypt.HashPassword(userRegisterDTO.Password);
                user.Gender = userRegisterDTO.Gender;
                user.DateOfBirth = userRegisterDTO.DateOfBirth;
                user.RoleId = userRegisterDTO.RoleId;
                user.Latitude = userRegisterDTO.Latitude;
                user.Longitude = userRegisterDTO.Longitude;
                user.Address = userRegisterDTO.Address;
                user.CreatedTime = DateTime.Now;

                _context.Add(user);
                await _context.SaveChangesAsync();

                return user;
            }
            catch (Exception err)
            {
                await Console.Out.WriteLineAsync(err.Message);
                return null;
            }
        }

        public async Task<bool> UpdateUserProfile(long userId, EditProfileDTO editProfileDTO)
        {
            try
            {
                var user = await _context.Users.FirstAsync(x => x.Id == userId);
                user.FullName = editProfileDTO.FullName;
                user.DateOfBirth = editProfileDTO.DateOfBirth;
                user.Gender = editProfileDTO.Gender;
                user.Address = editProfileDTO.Address;
                user.Longitude = editProfileDTO.Longitude;
                user.Latitude = editProfileDTO.Latitude;
                user.ProfileImage = editProfileDTO?.ProfileImage;

                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ResetUserPassword(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == email);

            try
            {
                user!.Password = BCrypt.Net.BCrypt.HashPassword(password);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int GetUserFollowerCount(long UserId)
        {
            return _context.UserFollowers.Where(x => x.UserId == UserId).Count();
        }

        public int GetUserFollowingCount(long UserId)
        {
            return _context.UserFollowers.Where(x => x.FollowerId == UserId).Count();
        }

        public int GetExpertTipsCount(long UserId)
        {
            return _context.Posts.Where(x => x.PostTypeId == 2 && x.UserId == UserId && x.Status).Count();
        }

        public ICollection<User> SearchByUsenameOrFullName(string name, long authId)
        {
            return _context.Users
                .Where(x => (x.RoleId == 2 || x.RoleId == 3) &&
                            (x.FullName.ToLower().StartsWith(name) || x.Username.ToLower().StartsWith(name)) &&
                            x.Id != authId)
                .ToList();
        }

        public async Task<User?> GetSearchUserById(long userId)
        {
            return await _context.Users.Where(x => x.RoleId == 2 || x.RoleId == 3).Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == userId);
        }

        public bool CheckIfIsFollowed(long authId, long id)
        {
            return _context.UserFollowers.Any(x => x.UserId == id && x.FollowerId == authId);
        }

        public async Task<bool> FollowUser(long authId, long userId)
        {
            try
            {
                var userFollow = new UserFollower
                {
                    UserId = userId,
                    FollowerId = authId,
                };

                _context.Add(userFollow);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UnfollowUser(long authId, long userId)
        {
            try
            {
                var userFollow = _context.UserFollowers.FirstOrDefault(x => x.UserId == userId && x.FollowerId == authId);
                _context.UserFollowers.Remove(userFollow!);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public User? GetActiveCatOwnerOrExpertByUsername(string username)
        {
            return _context.Users.FirstOrDefault(x => x.Username == username && x.RememberToken != null && (x.RoleId == 2 || x.RoleId == 3));
        }

        public List<User> GetOtherActiveCatOwnerAndExpert(long authId)
        {
            return _context.Users.Where(x => x.Id != authId && x.RememberToken != null && (x.RoleId == 2 || x.RoleId == 3)).ToList();
        }
    }
}
