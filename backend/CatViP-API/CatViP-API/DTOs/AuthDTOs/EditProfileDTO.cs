using System.ComponentModel.DataAnnotations;

namespace CatViP_API.DTOs.AuthDTOs
{
    public class EditProfileDTO
    {
        [Required]
        public string FullName { get; set; } = null!;
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public bool Gender { get; set; }
        public string Address { get; set; } = null!;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public byte[] ProfileImage { get; set; } = null!;
    }
}
