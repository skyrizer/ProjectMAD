using System;
using System.Collections.Generic;

namespace CatViP_API.Models;

public partial class ActionType
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<UserAction> UserActions { get; set; } = new List<UserAction>();
}
