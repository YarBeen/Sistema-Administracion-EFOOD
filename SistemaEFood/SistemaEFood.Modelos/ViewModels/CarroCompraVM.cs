using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.Modelos.ViewModels
{
    public class CarroCompraVM
    {
        public Producto Producto { get; set; }

        public CarroCompra CarroCompra { get; set; }

        public List<(string TipoPrecioNombre, float Monto)> ListaPrecios { get; set; } 

    }
}
