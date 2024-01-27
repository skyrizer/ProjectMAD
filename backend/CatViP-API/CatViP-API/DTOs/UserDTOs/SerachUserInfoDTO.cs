namespace CatViP_API.DTOs.UserDTOs
{
    public class SerachUserInfoDTO
    {
        public string Username { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public byte[]? ProfileImage { get; set; }
        public bool IsFollowed { get; set; }
        public int Follwer { get; set; }
        public int Following { get; set; }
        public bool IsExpert { get; set; }
        public int? ExpertTips { get; set; }
    }
}
