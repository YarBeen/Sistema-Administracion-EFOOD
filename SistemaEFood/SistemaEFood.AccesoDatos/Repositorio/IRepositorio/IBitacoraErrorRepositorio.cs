using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaEFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IBitacoraErrorRepositorio : IRepositorio<BitacoraError>
    {
        public Task RegistrarError(string mensaje, int numeroError);
        public Task<IEnumerable<BitacoraError>> ObtenerPorFecha(DateTime fecha);

        public Task<IEnumerable<BitacoraError>> ObtenerErroresEntreFechas(DateTime fechaInicio, DateTime fechaFin);


    }
}
