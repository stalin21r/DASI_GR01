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
    public DbSet<BranchEntity> Branches { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<ProductLoggerEntity> ProductLogs { get; set; }
    public DbSet<BalanceTransactionsEntity> BalanceTransactions { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<OrderDetailEntity> OrderDetails { get; set; }
    public DbSet<TopUpRequestEntity> TopUpRequests { get; set; }

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

      // Seed para Branches
      modelBuilder.Entity<BranchEntity>().HasData(
        new BranchEntity { Id = 1, Name = "Jefe Grupal" },
        new BranchEntity { Id = 2, Name = "Subjefe Grupal" },
        new BranchEntity { Id = 3, Name = "Manada" },
        new BranchEntity { Id = 4, Name = "Unidad Scout" },
        new BranchEntity { Id = 5, Name = "Caminantes" },
        new BranchEntity { Id = 6, Name = "Rovers" }
      );
      modelBuilder.Entity<BranchEntity>()
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

      modelBuilder.Entity<OrderEntity>()
          .HasOne<ApplicationUser>(o => o.Buyer)
          .WithMany()
          .HasForeignKey(o => o.BuyerId)
          .OnDelete(DeleteBehavior.Cascade); // Opcional: dejar este si quieres cascada

      modelBuilder.Entity<OrderEntity>()
          .HasOne<ApplicationUser>(o => o.Seller)
          .WithMany()
          .HasForeignKey(o => o.SellerId)
          .OnDelete(DeleteBehavior.Restrict); // Este desactiva la cascada para evitar el error

      modelBuilder.Entity<TopUpRequestEntity>()
          .HasOne<ApplicationUser>(t => t.AuthorizedByUser)
          .WithMany()
          .HasForeignKey(t => t.AuthorizedByUserId)
          .OnDelete(DeleteBehavior.Restrict); // o NoAction

      modelBuilder.Entity<TopUpRequestEntity>()
          .HasOne<ApplicationUser>(t => t.RequestedByUser)
          .WithMany()
          .HasForeignKey(t => t.RequestedByUserId)
          .OnDelete(DeleteBehavior.Restrict); // Cambiado de Cascade a Restrict

      modelBuilder.Entity<TopUpRequestEntity>()
          .HasOne<ApplicationUser>(t => t.TargetUser)
          .WithMany()
          .HasForeignKey(t => t.TargetUserId)
          .OnDelete(DeleteBehavior.Restrict); // Cambiado de Cascade a Restrict
    }
  }
}
