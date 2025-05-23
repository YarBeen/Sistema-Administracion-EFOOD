﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

        public DbSet<BitacoraError> BitacoraError { get; set; }

        public DbSet<CarroCompra> CarroCompra { get; set; }

        public DbSet<Orden> Ordenes { get; set; }

        public DbSet<OrdenDetalle> OrdenDetalle { get; set; }

        public DbSet<Bitacora> Bitacora { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
