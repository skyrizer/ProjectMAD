using System;
using System.Collections.Generic;

namespace CatViP_API.Models;

public partial class Comment
{
    public long Id { get; set; }

    public string Description { get; set; } = null!;

    public DateTime DateTime { get; set; }

    public long PostId { get; set; }

    public long UserId { get; set; }

    public virtual Post Post { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
