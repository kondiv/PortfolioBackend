using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class Project
{
    public Guid ProjectId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string GithubReference { get; set; } = null!;

    public int DevelopmentStatus { get; set; }

    public virtual ICollection<ProjectMedium> ProjectMedia { get; set; } = new List<ProjectMedium>();

    public virtual ICollection<Technology> Technologies { get; set; } = new List<Technology>();
}
