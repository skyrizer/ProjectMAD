using System;
using System.Collections.Generic;

namespace CatViP_API.Models;

public partial class Product
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string Description { get; set; } = null!;

    public bool Status { get; set; }

    public string URL { get; set; } = null!;

    public long SellerId { get; set; }

    public long ProductTypeId { get; set; }

    public byte[] Image { get; set; } = null!;

    public virtual ProductType ProductType { get; set; } = null!;

    public virtual User Seller { get; set; } = null!;
}
