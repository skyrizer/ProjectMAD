using System;
using System.Collections.Generic;

namespace CatViP_API.Models;

public partial class PostType
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

}
