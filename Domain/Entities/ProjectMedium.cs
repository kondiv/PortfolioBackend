using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class ProjectMedium
{
    public Guid ProjectMediaId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public int Order { get; set; }

    public Guid ProjectId { get; set; }

    public virtual Project Project { get; set; } = null!;
}
