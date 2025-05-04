// Backend/Data/ApplicationDbContext.cs
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend
{
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<OccupationEntity> Occupations { get; set; }
    public DbSet<ProductEntity> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // Default true para activo en usuarios
      modelBuilder.Entity<ApplicationUser>()
          .Property(u => u.Active)
          .HasDefaultValue(true);

      // Seed para Occupation
      modelBuilder.Entity<OccupationEntity>().HasData(
        new OccupationEntity { Id = 1, Name = "Jefe" },
        new OccupationEntity { Id = 2, Name = "Subjefe" },
        new OccupationEntity { Id = 3, Name = "Scout" }
      );
      modelBuilder.Entity<OccupationEntity>()
          .HasIndex(o => o.Name)
          .IsUnique();

      // Agrega propiedades de auditoría
      var auditableEntities = modelBuilder.Model.GetEntityTypes()
          .Where(e => e.ClrType.IsSubclassOf(typeof(AuditableEntity)));

      foreach (var auditableEntity in auditableEntities)
      {
        modelBuilder.Entity(auditableEntity.ClrType).Property<DateTime>("AuditableDate").HasDefaultValueSql("GETDATE()");
        modelBuilder.Entity(auditableEntity.ClrType).Property<string>("MachineName").HasDefaultValueSql("HOST_NAME()");
      }

      // Mapear el ProductType como string
      modelBuilder.Entity<ProductEntity>().Property(p => p.Type).HasConversion<string>();
    }

  }
}
