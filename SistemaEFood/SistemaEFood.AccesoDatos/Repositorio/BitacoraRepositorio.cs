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
    public class BitacoraRepositorio : Repositorio<Bitacora>, IBitacoraRepositorio
    {
        private readonly ApplicationDbContext _db;

        public BitacoraRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task RegistrarAccion(string nombreUsuario, string mensaje)
        {
            var accion = new Bitacora
            {
                Fecha = DateTime.Now,
                Hora = DateTime.Now.ToString("HH:mm:ss"),
                Mensaje = mensaje,
                nombreUsuario = nombreUsuario
            };
            _db.Bitacora.Add(accion);
            await _db.SaveChangesAsync();
        }
       
        public async Task<IEnumerable<Bitacora>> ObtenerPorFecha(DateTime fecha)
        {
            return await _db.Bitacora.Where(x => x.Fecha.Date == fecha.Date).ToListAsync();
        }


    }
}
