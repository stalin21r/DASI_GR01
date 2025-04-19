// Backend/Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using DASI_2025A.Backend.Data; // Asegúrate de que la ruta del namespace sea correcta
using DASI_2025A.Backend.Data.Models;

namespace DASI_2025A.Backend.Data
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Combos> Combos { get; set; } // Agrega aquí tus modelos
  }
}
