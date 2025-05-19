using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Models.Entities;

public partial class PortfolioContext : DbContext
{
    public PortfolioContext()
    {
    }

    public PortfolioContext(DbContextOptions<PortfolioContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectMedium> ProjectMedia { get; set; }

    public virtual DbSet<Technology> Technologies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=portfolio;Username=postgres;Password=Kondrashin2005");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("project_pk");

            entity.ToTable("project");

            entity.Property(e => e.ProjectId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("project_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DevelopmentStatus)
                .HasDefaultValue(0)
                .HasColumnName("development_status");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.GithubReference)
                .HasMaxLength(255)
                .HasColumnName("github_reference");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");

            entity.HasMany(d => d.Technologies).WithMany(p => p.Projects)
                .UsingEntity<Dictionary<string, object>>(
                    "ProjectTechnology",
                    r => r.HasOne<Technology>().WithMany()
                        .HasForeignKey("TechnologyId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("project_technology_technology_fk"),
                    l => l.HasOne<Project>().WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("project_technology_project_fk"),
                    j =>
                    {
                        j.HasKey("ProjectId", "TechnologyId").HasName("project_technology_pk");
                        j.ToTable("project_technology");
                        j.IndexerProperty<Guid>("ProjectId").HasColumnName("project_id");
                        j.IndexerProperty<Guid>("TechnologyId").HasColumnName("technology_id");
                    });
        });

        modelBuilder.Entity<ProjectMedium>(entity =>
        {
            entity.HasKey(e => e.ProjectMediaId).HasName("project_media_pk");

            entity.ToTable("project_media");

            entity.Property(e => e.ProjectMediaId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("project_media_id");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("image_url");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectMedia)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("project_media_project_fk");
        });

        modelBuilder.Entity<Technology>(entity =>
        {
            entity.HasKey(e => e.TechnologyId).HasName("technology_pk");

            entity.ToTable("technology");

            entity.Property(e => e.TechnologyId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("technology_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IconUrl)
                .HasMaxLength(255)
                .HasColumnName("icon_url");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
