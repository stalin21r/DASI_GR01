// Backend/Data/ApplicationDbContext.cs
using Backend.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend
{
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<ProductEntity> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

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
