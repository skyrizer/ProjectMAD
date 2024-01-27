using System;
using System.Collections.Generic;

namespace CatViP_API.Models;

public partial class UserAction
{
    public long Id { get; set; }

    public long PostId { get; set; }

    public long UserId { get; set; }

    public long ActionTypeId { get; set; }

    public virtual ActionType ActionType { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
