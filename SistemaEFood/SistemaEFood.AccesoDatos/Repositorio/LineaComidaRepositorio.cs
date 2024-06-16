using Microsoft.EntityFrameworkCore;
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
    public class LineaComidaRepositorio : Repositorio<LineaComida>, ILineaComidaRepositorio
    {
        private readonly ApplicationDbContext _db;

        public LineaComidaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(LineaComida lineaComida)
        {
            var lineaComidaBD = _db.LineasComida.FirstOrDefault(b => b.Id == lineaComida.Id);

            if (lineaComidaBD != null)
            {
				lineaComidaBD.Nombre = lineaComida.Nombre;
                _db.SaveChanges();
            }
        }
        public async Task<LineaComida> Obtener(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return await _db.LineasComida.FirstOrDefaultAsync(p => p.Id == id.Value);
        }
    }
}
