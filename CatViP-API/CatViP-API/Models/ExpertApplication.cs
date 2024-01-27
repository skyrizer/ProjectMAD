using System;
using System.Collections.Generic;

namespace CatViP_API.Models;

public partial class ExpertApplication
{
    public long Id { get; set; }

    public string Description { get; set; } = null!;

    public byte[] Documentation { get; set; } = null!;

    public string? RejectedReason { get; set; }

    public DateTime DateTime { get; set; }

    public DateTime? DateTimeUpdated { get; set; }

    public long StatusId { get; set; }

    public long UserId { get; set; }

    public virtual ExpertApplicationStatus Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
