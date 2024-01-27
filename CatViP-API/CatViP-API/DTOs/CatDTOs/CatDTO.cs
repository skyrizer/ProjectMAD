namespace CatViP_API.DTOs.CatDTOs
{
    public class CatDTO
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        public DateTime DateTimeCreated { get; set; }

        public bool Gender { get; set; }

        public byte[]? ProfileImage { get; set; }
    }
}
