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
    public DbSet<ProductLoggerEntity> ProductLogs { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<OrderDetailEntity> OrderDetails { get; set; }

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
      
      // Para consultad que involucren UserId y OrderDate
      modelBuilder.Entity<OrderEntity>().HasIndex(o => new { o.UserId, o.OrderDate });
      // Restringe valores de Quantity < 0
      modelBuilder.Entity<OrderDetailEntity>(entity =>
      {
        entity.ToTable(tb => tb.HasCheckConstraint("CK_OrderDetail_Quantity", "[Quantity] > 0"));
      });
      // Convertir enum en string para guardar en la BD
      modelBuilder.Entity<PaymentEntity>().Property(o => o.PaymentMethod).HasConversion<string>().HasMaxLength(20);
            modelBuilder.Entity<OrderEntity>().Property(o => o.Status).HasConversion<string>().HasMaxLength(20);
      // Restringe TotalAmount >= 0
      modelBuilder.Entity<OrderEntity>(entity =>
      {
          entity.ToTable(tb => tb.HasCheckConstraint("CK_Order_TotalAmount_Positive", "[TotalAmount] >= 0"));
      });
      // Columna Subtotal calculada
      modelBuilder.Entity<OrderDetailEntity>()
        .Property(e => e.SubTotal)
        .HasColumnType("decimal(18,2)")
        .ValueGeneratedOnAddOrUpdate()
        .HasComputedColumnSql("[Quantity] * [UnitPrice]");

      // Muestra registros activos
      modelBuilder.Entity<OrderEntity>().HasQueryFilter(e => e.IsActive);
      modelBuilder.Entity<OrderDetailEntity>().HasQueryFilter(e => e.IsActive);
    }

  }
}
