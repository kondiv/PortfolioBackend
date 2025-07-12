using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Technology
{
    public Guid TechnologyId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string IconUrl { get; set; } = null!;

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
