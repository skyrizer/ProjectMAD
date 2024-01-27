using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatViP_API.Models;

public partial class CatCaseReport
{
    public long Id { get; set; }

    public string Description { get; set; } = null!;

    public string Address { get; set; } = null!;
    [Column(TypeName = "decimal(9,6)")]
    public decimal Longitude { get; set; }
    [Column(TypeName = "decimal(9,6)")]
    public decimal Latitude { get; set; }

    public long UserId { get; set; }

    public long? CatId { get; set; }

    public long CatCaseReportStatusId { get; set; }

    public DateTime DateTime { get; set; }

    public virtual Cat? Cat { get; set; }

    public virtual ICollection<CatCaseReportImage> CatCaseReportImages { get; set; } = new List<CatCaseReportImage>();

    public virtual CatCaseReportStatus CatCaseReportStatus { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual ICollection<CatCaseReportComment> Comments { get; set; } = new List<CatCaseReportComment>();
}
