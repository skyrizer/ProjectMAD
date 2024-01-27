using System.ComponentModel.DataAnnotations;

namespace CatViP_API.DTOs.CatDTOs
{
    public class CatRequestDTO
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public bool Gender { get; set; }
        [Required]
        public byte[]? ProfileImage { get; set; }
    }
}
