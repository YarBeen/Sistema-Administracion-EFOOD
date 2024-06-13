using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaEFood.Modelos;
using SistemaEFood.Modelos.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IOrdenDetalleRepositorio : IRepositorio<OrdenDetalle>
    {
        void Actualizar (OrdenDetalle ordenDetalle);
        Task<IEnumerable<OrdenDetalle>> ObtenerPorFecha(DateTime fecha);
        Task<IEnumerable<OrdenDetalle>> ObtenerEntreFechas(DateTime fechaInicio, DateTime fechaFin);

        Task<IEnumerable<OrdenDetalle>> ObtenerPedidosPorEstado(string estado);
    }
}
