using System;
using System.Collections.Generic;

namespace CatViP_API.Models;

public partial class UserFollower
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long FollowerId { get; set; }

    public virtual User Follower { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
