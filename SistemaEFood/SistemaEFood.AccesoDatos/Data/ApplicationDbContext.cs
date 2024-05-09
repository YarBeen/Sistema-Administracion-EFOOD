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
        public DbSet<TipoPrecio> TiposPrecio { get; set; }

        public DbSet<ProcesadorDePago> ProcesadorDePago { get; set; }
		public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<LineaComida> LineasComida { get; set; }

        public DbSet<Producto> Productos { get; set; }

        public DbSet<ProcesadorTarjeta> ProcesadorTarjeta { get; set; }

        public DbSet<TiqueteDeDescuento> TiqueteDeDescuento { get; set; }

        public DbSet<ProductoPrecio> ProductoPrecio { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
