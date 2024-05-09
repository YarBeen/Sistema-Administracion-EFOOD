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
        public ProductoPrecio productoPrecio { get; set; }
        public IEnumerable<SelectListItem> ListaPrecios { get; set; }

    }
}
