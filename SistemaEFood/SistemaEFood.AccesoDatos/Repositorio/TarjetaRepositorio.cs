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
    public class TarjetaRepositorio : Repositorio<Tarjeta>, ITarjetaRepositorio
    {
        private readonly ApplicationDbContext _db;

        public TarjetaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(Tarjeta tarjeta)
        {
            var bodegaBD = _db.Tarjetas.FirstOrDefault(b => b.Id == tarjeta.Id);

            if (bodegaBD != null)
            {
                bodegaBD.Nombre = tarjeta.Nombre;
                _db.SaveChanges();
            }
        }
    }
}
