using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Domain.Entities;

public partial class PortfolioContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public PortfolioContext()
    {
    }

    public PortfolioContext(DbContextOptions<PortfolioContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<ProjectMedium> ProjectMedia { get; set; }

    public virtual DbSet<Technology> Technologies { get; set; }
    
    public virtual DbSet<UserSkill> UserSkills { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.SkillId);

            entity.Property(e => e.SkillId)
                .ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<UserSkill>(entity =>
        {
            entity.HasKey(e => e.UserSkillId);

            entity.Property(e => e.UserSkillId)
                .ValueGeneratedOnAdd();

            entity.HasOne(e => e.Skill)
                .WithMany(s => s.UserSkills)
                .HasForeignKey(e => e.SkillId);

            entity.HasOne(e => e.User)
                .WithMany(u => u.UserSkills)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("RefreshToken");
            
            entity.HasKey(e => e.RefreshTokenId);

            entity.Property(e => e.RefreshTokenId)
                .HasDefaultValueSql("uuid_generate_v4()");

            entity.HasOne(e => e.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionId);

            entity.ToTable("permission");

            entity.Property(e => e.PermissionId)
                .ValueGeneratedOnAdd()
                .HasColumnName("permission_id");
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => new { e.PermissionId, e.RoleId});

            entity.ToTable("role_permission");

            entity.Property(e => e.PermissionId)
                .ValueGeneratedOnAdd()
                .HasColumnName("permission_id");

            entity.HasOne(e => e.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(e => e.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

        });

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
