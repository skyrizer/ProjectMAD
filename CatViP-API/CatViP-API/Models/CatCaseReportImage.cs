using System;
using System.Collections.Generic;

namespace CatViP_API.Models;

public partial class CatCaseReportImage
{
    public long Id { get; set; }

    public byte[] Image { get; set; } = null!;

    public bool IsBloodyContent { get; set; }

    public long CatCaseReportId { get; set; }

    public virtual CatCaseReport CatCaseReport { get; set; } = null!;
}
