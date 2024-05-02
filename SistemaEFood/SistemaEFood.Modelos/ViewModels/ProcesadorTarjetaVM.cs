using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.Modelos.ViewModels
{
    public class ProcesadorTarjetaVM
    {
        public ProcesadorTarjeta ProcesadorTarjeta { get; set; }
        public IEnumerable<SelectListItem> TarjetaLista { get; set; }

    }
}
