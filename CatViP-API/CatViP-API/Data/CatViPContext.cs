using CatViP_API.Models;
using Microsoft.EntityFrameworkCore;

namespace CatViP_API.Data
{
    public partial class CatViPContext : DbContext
    {
        public CatViPContext()
        {
        }

        public CatViPContext(DbContextOptions<CatViPContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActionType> ActionTypes { get; set; }

        public virtual DbSet<Cat> Cats { get; set; }

        public virtual DbSet<CatCaseReportComment> CatCaseReportComments { get; set; }

        public virtual DbSet<CatCaseReport> CatCaseReports { get; set; }

        public virtual DbSet<CatCaseReportImage> CatCaseReportImages { get; set; }

        public virtual DbSet<CatCaseReportStatus> CatCaseReportStatuses { get; set; }

        public virtual DbSet<Chat> Chats { get; set; }

        public virtual DbSet<UserChat> UserChats { get; set; }

        public virtual DbSet<Comment> Comments { get; set; }

        public virtual DbSet<ExpertApplication> ExpertApplications { get; set; }

        public virtual DbSet<ExpertApplicationStatus> ExpertApplicationStatuses { get; set; }

        public virtual DbSet<MentionedCat> MentionedCats { get; set; }

        public virtual DbSet<Post> Posts { get; set; }

        public virtual DbSet<PostImage> PostImages { get; set; }

        public virtual DbSet<PostReport> PostReports { get; set; }

        public virtual DbSet<PostType> PostTypes { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<ProductType> ProductTypes { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<UserAction> UserActions { get; set; }

        public virtual DbSet<UserFollower> UserFollowers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cat>(entity =>
            {
                entity.Property(e => e.DateOfBirth).HasColumnType("date");
                entity.Property(e => e.DateTimeCreated).HasColumnType("datetime");

                entity.HasOne(d => d.User).WithMany(p => p.Cats)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cats_Users");
            });

            modelBuilder.Entity<CatCaseReport>(entity =>
            {
                entity.HasOne(d => d.CatCaseReportStatus).WithMany(p => p.CatCaseReports)
                    .HasForeignKey(d => d.CatCaseReportStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CatCaseReports_CatCaseReportStatuses");

                entity.HasOne(d => d.Cat).WithMany(p => p.CatCaseReports)
                    .HasForeignKey(d => d.CatId)
                    .HasConstraintName("FK_CatCaseReports_Cats");

                entity.HasOne(d => d.User).WithMany(p => p.CatCaseReports)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CatCaseReports_Users");
            });

            modelBuilder.Entity<CatCaseReportComment>(entity =>
            {
                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.HasOne(d => d.CatCaseReport).WithMany(p => p.Comments)
                    .HasForeignKey(d => d.CatCaseReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CatCaseReportComment_CatCaseReport");

                entity.HasOne(d => d.User).WithMany(p => p.CatCaseReportComments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CatCaseReportComment_Users");
            });

            modelBuilder.Entity<CatCaseReportImage>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_CatCaseImages");

                entity.HasOne(d => d.CatCaseReport).WithMany(p => p.CatCaseReportImages)
                    .HasForeignKey(d => d.CatCaseReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CatCaseImages_CatCaseReports");
            });

            modelBuilder.Entity<Chat>(entity =>
            {
                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.HasOne(d => d.UserChat).WithMany(p => p.Chats)
                    .HasForeignKey(d => d.UserChatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Chats_UserChats");
            });

            modelBuilder.Entity<UserChat>(entity =>
            {
                entity.Property(e => e.LastSeen).HasColumnType("datetime");

                entity.HasOne(d => d.UserReceive).WithMany(p => p.ChatUserReceives)
                    .HasForeignKey(d => d.UserReceiveId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserChats_Users1");

                entity.HasOne(d => d.UserSend).WithMany(p => p.ChatUserSends)
                    .HasForeignKey(d => d.UserSendId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserChats_Users");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Post).WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comments_Posts");

                entity.HasOne(d => d.User).WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comments_Users");
            });

            modelBuilder.Entity<ExpertApplication>(entity =>
            {
                entity.HasOne(d => d.Status).WithMany(p => p.ExpertApplications)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExpertApplications_ExpertApplicationStatusTypes");

                entity.HasOne(d => d.User).WithMany(p => p.ExpertApplications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExpertApplications_Users");
            });

            modelBuilder.Entity<ExpertApplicationStatus>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_ExpertApplicationStatusTypes");
            });

            modelBuilder.Entity<MentionedCat>(entity =>
            {
                entity.HasOne(d => d.Cat).WithMany(p => p.MentionedCats)
                    .HasForeignKey(d => d.CatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MentionedCats_Cats");

                entity.HasOne(d => d.Post).WithMany(p => p.MentionedCats)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MentionedCats_Posts");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.HasOne(d => d.PostType).WithMany(p => p.Posts)
                    .HasForeignKey(d => d.PostTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Posts_PostTypes");

                entity.HasOne(d => d.User).WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Posts_Users");
            });

            modelBuilder.Entity<PostImage>(entity =>
            {
                entity.HasOne(d => d.Post).WithMany(p => p.PostImages)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostImages_Posts");
            });

            modelBuilder.Entity<PostReport>(entity =>
            {
                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.HasOne(d => d.User).WithMany(p => p.PostReports)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostReports_Users");

                entity.HasOne(d => d.Post).WithMany(p => p.PostReports)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostReports_Posts");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.ProductType).WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Products_ProductTypes");

                entity.HasOne(d => d.Seller).WithMany(p => p.Products)
                    .HasForeignKey(d => d.SellerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Products_Users");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.DateOfBirth).HasColumnType("date");
                entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");
                entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
                entity.Property(e => e.TokenCreated).HasColumnType("datetime");
                entity.Property(e => e.TokenExpires).HasColumnType("datetime");

                entity.HasOne(d => d.Role).WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Roles");
            });

            modelBuilder.Entity<UserAction>(entity =>
            {
                entity.HasOne(d => d.ActionType).WithMany(p => p.UserActions)
                    .HasForeignKey(d => d.ActionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserActions_ActionTypes");

                entity.HasOne(d => d.Post).WithMany(p => p.UserActions)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserActions_Post");

                entity.HasOne(d => d.User).WithMany(p => p.UserActions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserActions_Users");
            });

            modelBuilder.Entity<UserFollower>(entity =>
            {
                entity.HasOne(d => d.Follower).WithMany(p => p.UserFollowerFollowers)
                    .HasForeignKey(d => d.FollowerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserFollowers_Users1");

                entity.HasOne(d => d.User).WithMany(p => p.UserFollowerUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserFollowers_Users");
            });

            modelBuilder.Entity<User>()
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(x => x.Username)
                .IsUnique();

            OnModelCreatingPartial(modelBuilder);
            new DbInitializer(modelBuilder).Seed();
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

}
