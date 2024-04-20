using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaEFood.Modelos;
using System.Reflection;

namespace SistemaEFood.AccesoDatos.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Tarjeta> Tarjetas { get; set; }
        public DbSet<ProcesadorDePago> ProcesadorDePago { get; set; }
		public DbSet<Usuario> Usuarios { get; set; }


		protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
