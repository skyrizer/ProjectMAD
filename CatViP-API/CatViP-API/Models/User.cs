using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatViP_API.Models;

public partial class User
{
    public long Id { get; set; }

    public string Username { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool Gender { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string? Address { get; set; }

    public byte[]? ProfileImage { get; set; }
    [Column(TypeName = "decimal(9,6)")]
    public decimal? Longitude { get; set; }
    [Column(TypeName = "decimal(9,6)")]
    public decimal? Latitude { get; set; }

    public DateTime CreatedTime { get; set; }

    public string? RememberToken { get; set; }

    public DateTime? TokenCreated { get; set; }

    public DateTime? TokenExpires { get; set; }
    
    public long RoleId { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<CatCaseReport> CatCaseReports { get; set; } = new List<CatCaseReport>();

    public virtual ICollection<Cat> Cats { get; set; } = new List<Cat>();

    public virtual ICollection<CatCaseReportComment> CatCaseReportComments { get; set; } = new List<CatCaseReportComment>();

    public virtual ICollection<UserChat> ChatUserReceives { get; set; } = new List<UserChat>();

    public virtual ICollection<UserChat> ChatUserSends { get; set; } = new List<UserChat>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<ExpertApplication> ExpertApplications { get; set; } = new List<ExpertApplication>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<PostReport> PostReports { get; set; } = new List<PostReport>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<UserAction> UserActions { get; set; } = new List<UserAction>();

    public virtual ICollection<UserFollower> UserFollowerFollowers { get; set; } = new List<UserFollower>();

    public virtual ICollection<UserFollower> UserFollowerUsers { get; set; } = new List<UserFollower>();
}
