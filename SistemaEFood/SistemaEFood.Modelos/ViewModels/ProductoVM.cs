using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.Modelos.ViewModels
{
    public class ProductoVM
    {
        public Producto Producto { get; set; }

        public IEnumerable<SelectListItem> LineaComidaLista {  get; set; } 

        public IEnumerable<Producto> ProductosLista { get; set; }
    }
}
