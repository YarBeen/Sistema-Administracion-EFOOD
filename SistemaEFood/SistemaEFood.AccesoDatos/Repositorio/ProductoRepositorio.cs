using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaEFood.AccesoDatos.Data;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Modelos;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.AccesoDatos.Repositorio
{
    public class ProductoRepositorio : Repositorio<Producto>, IProductoRepositorio
    {
        private readonly ApplicationDbContext _db;

        public ProductoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(Producto producto)
        {
            var productoBD = _db.Productos.FirstOrDefault(b => b.Id == producto.Id);

            if (productoBD != null)
            {
                if(producto.ImagenUrl != null)
                {
                    productoBD.ImagenUrl = producto.ImagenUrl;
                }

                productoBD.Nombre = producto.Nombre;
                productoBD.Contenido = producto.Contenido;
                productoBD.LineaComidaId = producto.LineaComidaId;
                productoBD.PadreId = producto.PadreId;
                _db.SaveChanges();
            }
        }

        public IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj)
        {
            if(obj == "LineaComida")
            {
                return _db.LineasComida.Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });
            }
            if (obj == "Producto")
            {
                return _db.Productos.Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });
            }
            return null;
        }


        public async Task<IEnumerable<Producto>> ObtenerTodosPorLineaComida(int idLineaComida)
        {
            var productos = await _db.Productos
                .Include(p => p.LineaComida)
                .Where(p => p.LineaComidaId == idLineaComida)
                .ToListAsync();
            return productos;
        }


        public async Task<IEnumerable<(string TipoPrecioNombre, float Monto)>> ObtenerPreciosPorTamanno (int productoId)
        {
            var resultados = await _db.ProductoPrecio
                .Where(pp => pp.Idproducto == productoId)
                .Select(pp => new
                {
                    TipoPrecioNombre = pp.TipoPrecio.Nombre,
                    pp.Monto
                })
                .ToListAsync();

            return resultados.Select(r => (r.TipoPrecioNombre, r.Monto));
        }

    }



}
