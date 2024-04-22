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
    public class TipoPrecioRepositorio : Repositorio<TipoPrecio>, ITipoPrecioRepositorio
    {
        private readonly ApplicationDbContext _db;

        public TipoPrecioRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(TipoPrecio tipoPrecio)
        {
            var tipoPrecioBD = _db.TiposPrecio.FirstOrDefault(b => b.Id == tipoPrecio.Id);

            if (tipoPrecioBD != null)
            {
                tipoPrecioBD.Nombre = tipoPrecio.Nombre;
                _db.SaveChanges();
            }
        }
    }
}
