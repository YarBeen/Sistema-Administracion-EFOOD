using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.Modelos.ViewModels
{
    public class ProductoPrecioVM
    {
        public int idProducto { get; set; } 
        public int idRelacion {  get; set; }
        public int tipoPrecioID {  get; set; }

        public string tipoPrecioNombre {  get; set; }

        public int monto { get; set; }
        public IEnumerable<SelectListItem> ListaPrecios { get; set; }

    }
}
