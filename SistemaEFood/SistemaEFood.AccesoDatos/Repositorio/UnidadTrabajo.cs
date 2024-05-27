using Microsoft.AspNetCore.Identity;
using SistemaEFood.AccesoDatos.Data;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.AccesoDatos.Repositorio
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        private readonly ApplicationDbContext _db;
        public ITarjetaRepositorio Tarjeta { get; private set; }
        public IProcesadorDePagoRepositorio ProcesadorDePago { get; private set; }

        public ITiqueteDeDescuentoRepositorio TiqueteDeDescuento { get; private set; }
        public IUsuarioRepositorio Usuario { get; private set; }

        public ILineaComidaRepositorio LineaComida { get; private set; }
        public ITipoPrecioRepositorio TipoPrecio { get; private set; }

        public IProductoRepositorio Producto { get; private set; }

        public IProcesadorTarjeta ProcesadorTarjeta { get; private set; }

        public IProductoPrecioRepositorio ProductoPrecio { get; private set; }
        public UnidadTrabajo(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            Tarjeta = new TarjetaRepositorio(_db);
            TiqueteDeDescuento = new TiqueteDeDescuentoRepositorio(_db);
            ProcesadorDePago = new ProcesadorDePagoRepositorio(_db);
            Usuario = new UsuarioRepositorio(_db,userManager);
            LineaComida = new LineaComidaRepositorio(_db);
            TipoPrecio = new TipoPrecioRepositorio(_db);
            Producto = new ProductoRepositorio(_db);
            ProcesadorTarjeta = new ProcesadorTarjetaRepositorio(db);
            ProductoPrecio = new ProductoPrecioRepositorio(db);
        }
        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task Guardar()
        {
           await _db.SaveChangesAsync();
        }
    }
}
