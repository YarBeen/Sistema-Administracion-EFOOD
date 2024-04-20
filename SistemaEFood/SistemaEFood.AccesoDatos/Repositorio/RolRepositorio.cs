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
    public class RolRepositorio : Repositorio<Rol>, IRolRepositorio
    {
        private readonly ApplicationDbContext _db;

        public RolRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(Rol rol)
        {
            var rolBD = _db.Roles.FirstOrDefault(b => b.Id == rol.Id);

            if (rolBD != null)
            {
                rolBD.Nombre = rol.Nombre;
                _db.SaveChanges();
            }
        }
    }
}
