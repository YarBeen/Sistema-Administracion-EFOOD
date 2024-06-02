using SistemaEFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IBitacoraRepositorio : IRepositorio<Bitacora>
    {
        public Task RegistrarAccion(string nombreUsuario, string mensaje);
        public Task<IEnumerable<Bitacora>> ObtenerPorFecha(DateTime fecha);
    }
}
