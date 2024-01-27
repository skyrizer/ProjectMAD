namespace CatViP_API.DTOs.AuthDTOs
{
    public class AuthInfoDTO
    {
        public string Username { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public byte[]? ProfileImage { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int Follwer { get; set; }
        public int Following { get; set; }
        public bool IsExpert { get; set; }
        public int? ExpertTips { get; set; }
    }
}
