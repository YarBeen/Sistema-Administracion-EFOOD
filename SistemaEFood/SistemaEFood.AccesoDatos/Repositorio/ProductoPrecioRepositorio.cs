using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class ProductoPrecioRepositorio : Repositorio<ProductoPrecio>, IProductoPrecioRepositorio
    {
        private readonly ApplicationDbContext _db;
        public ProductoPrecioRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(ProductoPrecio productoPrecio)
        {
            var ProductoPrecioDB = _db.ProductoPrecio.FirstOrDefault(c => c.Id == productoPrecio.Id);
            if(ProductoPrecioDB != null) 
            {
                ProductoPrecioDB.Idprecio = productoPrecio.Idprecio;
                ProductoPrecioDB.Idproducto = productoPrecio.Idproducto;
                ProductoPrecioDB.Monto = productoPrecio.Monto;
                _db.SaveChanges();
            }
        }

        public IEnumerable<SelectListItem> ObtenerTipoPrecios(string obj, int? idProducto)
        {
            if (obj.Equals("TipoPrecio"))
            {
                var tipoPrecios = _db.TiposPrecio
                                    .Where(t => !_db.ProductoPrecio
                                                    .Any(pt => pt.Idprecio == t.Id && pt.Idproducto == idProducto))
                                    .Select(c => new SelectListItem
                                    {
                                        Text = c.Nombre,
                                        Value = c.Id.ToString()
                                    })
                                    .ToList();
                return tipoPrecios;
            }
            return null;
        }
    }
}
