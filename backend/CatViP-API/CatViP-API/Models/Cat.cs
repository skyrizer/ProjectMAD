using System;
using System.Collections.Generic;

namespace CatViP_API.Models;

public partial class Cat
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public DateTime DateTimeCreated { get; set; }

    public bool Gender { get; set; }

    public byte[]? ProfileImage { get; set; }
    public bool Status { get; set; }

    public long UserId { get; set; }

    public virtual ICollection<CatCaseReport> CatCaseReports { get; set; } = new List<CatCaseReport>();

    public virtual ICollection<MentionedCat> MentionedCats { get; set; } = new List<MentionedCat>();

    public virtual User User { get; set; } = null!;
}
