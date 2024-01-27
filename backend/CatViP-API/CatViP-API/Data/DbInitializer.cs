using CatViP_API.Helpers;
using CatViP_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Transactions;

namespace CatViP_API.Data
{
    public class DbInitializer
    {
        private readonly ModelBuilder modelBuilder;

        public DbInitializer(ModelBuilder modelBuilder)
        {
            this.modelBuilder = modelBuilder;
        }

        public void Seed()
        {
            modelBuilder.Entity<ActionType>().HasData(
                new ActionType() { Id = 1, Name = "Like"},
                new ActionType() { Id = 2, Name = "DisLike" }
            );

            modelBuilder.Entity<CatCaseReportStatus>().HasData(
                new CatCaseReportStatus() { Id = 1, Name = "Pending" },
                new CatCaseReportStatus() { Id = 2, Name = "Settled" },
                new CatCaseReportStatus() { Id = 3, Name = "Revoked" }
            );

            modelBuilder.Entity<ExpertApplicationStatus>().HasData(
                new ExpertApplicationStatus() { Id = 1, Name = "Success" },
                new ExpertApplicationStatus() { Id = 2, Name = "Pending" },
                new ExpertApplicationStatus() { Id = 3, Name = "Rejected" },
                new ExpertApplicationStatus() { Id = 4, Name = "Revoked" }
            );

            modelBuilder.Entity<PostType>().HasData(
                new PostType() { Id = 1, Name = "Daily sharing" },
                new PostType() { Id = 2, Name = "Expert tip" }
            );

            modelBuilder.Entity<ProductType>().HasData(
                new ProductType() { Id = 1, Name = "Food" },
                new ProductType() { Id = 2, Name = "Collar" },
                new ProductType() { Id = 3, Name = "Health care" },
                new ProductType() { Id = 4, Name = "Toy" },
                new ProductType() { Id = 5, Name = "Litter and tray" },
                new ProductType() { Id = 6, Name = "Bowl" }
            );

            modelBuilder.Entity<Role>().HasData(
                new Role() { Id = 1, Name = "System Admin" },
                new Role() { Id = 2, Name = "Cat Owner" },
                new Role() { Id = 3, Name = "Cat Expert" },
                new Role() { Id = 4, Name = "Cat Product Seller" }
            );

            modelBuilder.Entity<User>().HasData(
                new User() { Id = 1, Username = "admin", FullName = "CatViP Admin", Email = "admin@catvip.my", Password = BCrypt.Net.BCrypt.HashPassword("abc12345"), Gender = true, DateOfBirth = new DateTime(2000, 1, 1), CreatedTime = new DateTime(2023, 12, 1), RoleId = 1 },
                new User() { Id = 2, Username = "stephen", FullName = "stephen sim", Email = "simshansiong2002@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("abc12345"), Gender = true, DateOfBirth = new DateTime(2000, 1, 1), RoleId = 2, Address = "UTeM, Jalan Hang Tuah Jaya, 76100 Durian Tunggal, Melaka", Latitude = 2.3164m, Longitude = 102.3208m, CreatedTime = new DateTime(2023, 12, 1) },
                new User() { Id = 3, Username = "tong", FullName = "yung huey", Email = "tong@catvip.my", Password = BCrypt.Net.BCrypt.HashPassword("abc12345"), Gender = false, DateOfBirth = new DateTime(2000, 1, 1), RoleId = 3, Address = "UTeM, Jalan Hang Tuah Jaya, 76100 Durian Tunggal, Melaka", Latitude = 2.3164m, Longitude = 102.3208m, CreatedTime = new DateTime(2023, 12, 1) },
                new User() { Id = 4, Username = "wafir", FullName = "wafir the best", Email = "wafir@catvip.my", Password = BCrypt.Net.BCrypt.HashPassword("abc12345"), Gender = true, DateOfBirth = new DateTime(2000, 1, 1), RoleId = 4, CreatedTime = new DateTime(2023, 12, 1), }
            );

            modelBuilder.Entity<ExpertApplication>().HasData(
                new ExpertApplication() { Id = 1, Description = "I am a cat doctor", DateTime = DateTime.Now, DateTimeUpdated = DateTime.Now, StatusId = 1, UserId = 3, Documentation = ConvertFileToByteArrayHelper.ConvertPDFFileToByteArray("tong-vet.pdf") }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product() { Id = 1, Name = "Minkas Junior Care", Price = 41.00m, Description = "Our Happy Cat Minkas Junior Care is provided with valuable proteins and high-quality ingredients such as omega-3 and omega-6 fatty acids.", Status = true, URL = "https://www.happypetmalaysia.com/SalePage/Index/287683", SellerId = 4, ProductTypeId = 1, Image = ConvertFileToByteArrayHelper.ConvertImageFileToByteArray("minkas-junior.jpg") },
                new Product() { Id = 2, Name = "Royal Canin Bowl", Price = 12.00m, Description = "This versatile dish has an accent-patterned outer dish with a stainless steel dish insert.", Status = true, URL = "https://shp.ee/i8viwu2", SellerId = 4, ProductTypeId = 6, Image = ConvertFileToByteArrayHelper.ConvertImageFileToByteArray("royal-chin.jpg") },
                new Product() { Id = 3, Name = "Catit Play", Price = 29.00m, Description = "The Catit Play Circuit Ball Toy is a 3 in 1 activity toy that will entertain your kitty with hours of playtime. The 3 in 1 activity toy includes a: Multi massager with combs, circuit ball toy, a bouncy bee and catnip.", Status = true, URL = "https://shp.ee/pk74sfe", SellerId = 4, ProductTypeId = 4, Image = ConvertFileToByteArrayHelper.ConvertImageFileToByteArray("catit-play.jpg") }
            );
        }
    }
}
