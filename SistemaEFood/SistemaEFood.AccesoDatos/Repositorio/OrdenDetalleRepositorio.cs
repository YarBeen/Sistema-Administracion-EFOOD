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
    public class OrdenDetalleRepositorio : Repositorio<OrdenDetalle>, IOrdenDetalleRepositorio
    {
        private readonly ApplicationDbContext _db;

        public OrdenDetalleRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(OrdenDetalle ordenDetalle)
        {
            _db.Update(ordenDetalle);
        }

        public async Task<IEnumerable<OrdenDetalle>> ObtenerPorFecha(DateTime fecha)
        {
            return await _db.OrdenDetalle.Where(x => x.FechaOrden.Date == fecha.Date).ToListAsync();
        }
        public async Task<IEnumerable<OrdenDetalle>> ObtenerEntreFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            var fechaFinInclusive = fechaFin.AddDays(1).AddTicks(-1);
            var datos = await _db.OrdenDetalle
                                        .Where(be => be.FechaOrden >= fechaInicio && be.FechaOrden <= fechaFinInclusive)
                                        .ToListAsync();
            return datos;
        }

        public async Task<IEnumerable<OrdenDetalle>> ObtenerPedidosPorEstado(string estado)
        {
            return await _db.OrdenDetalle
                        .Where(p => p.Estado == estado)
                        .ToListAsync();
        }
    }
}
