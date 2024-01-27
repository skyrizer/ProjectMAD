using System.ComponentModel.DataAnnotations;

namespace CatViP_API.DTOs.AuthDTOs
{
    public class UserLoginRequestDTO
    {
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public bool IsMobileLogin { get; set; }
    }
}
