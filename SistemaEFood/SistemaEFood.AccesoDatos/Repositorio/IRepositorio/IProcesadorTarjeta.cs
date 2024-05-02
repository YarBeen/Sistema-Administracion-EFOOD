using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaEFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IProcesadorTarjeta : IRepositorio<ProcesadorTarjeta>
    {
        void Actualizar(ProcesadorTarjeta procesadorTarjeta);

        public IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj, int? idProcesador);


    }
}
