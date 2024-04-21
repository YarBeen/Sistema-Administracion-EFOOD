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
    public class TiqueteDeDescuentoRepositorio : Repositorio<TiqueteDeDescuento>, ITiqueteDeDescuentoRepositorio
    {
        private readonly ApplicationDbContext _db;

        public TiqueteDeDescuentoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(TiqueteDeDescuento tiqueteDeDescuento)
        {
            var tiqueteDeDescuentoBD = _db.TiqueteDeDescuento.FirstOrDefault(b => b.Id == tiqueteDeDescuento.Id);

            if (tiqueteDeDescuentoBD != null)
            {
                tiqueteDeDescuentoBD.Nombre = tiqueteDeDescuento.Nombre;
                tiqueteDeDescuentoBD.Codigo = tiqueteDeDescuento.Codigo;
                tiqueteDeDescuentoBD.Disponibles = tiqueteDeDescuento.Disponibles;
                tiqueteDeDescuentoBD.Descuento = tiqueteDeDescuento.Descuento;
 

                _db.SaveChanges();
            }
        }
    }
}
