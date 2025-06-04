using DoDo.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DoDo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ya con [Table("Usuarios")] no es estrictamente necesario
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.CorreoElectronico)
                .IsUnique();
        }
    } 
}

