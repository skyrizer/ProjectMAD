using System;
using System.Collections.Generic;

namespace CatViP_API.Models;

public partial class ExpertApplicationStatus
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ExpertApplication> ExpertApplications { get; set; } = new List<ExpertApplication>();
}
