using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.Modelos.ViewModels
{
    public class PedidoVM
    {
        public IEnumerable<OrdenDetalle> ListaOrdenDetalles { get; set; }
    }
}
