using System;
using System.Collections.Generic;

namespace CatViP_API.Models;

public partial class MentionedCat
{
    public long Id { get; set; }

    public long PostId { get; set; }

    public long CatId { get; set; }

    public virtual Cat Cat { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;
}
