// Backend/Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using Backend.Data; // Asegúrate de que la ruta del namespace sea correcta
using Backend.Data.Models;

namespace Backend.Data
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Combos> Combos { get; set; } // Agrega aquí tus modelos
  }
}
