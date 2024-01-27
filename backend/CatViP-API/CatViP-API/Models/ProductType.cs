using System;
using System.Collections.Generic;

namespace CatViP_API.Models;

public partial class ProductType
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
