using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaEFood.Modelos.Especificaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.Modelos.ViewModels
{
    public class ProductoBusquedaVM
    {
        public PagedList<Producto> Productos { get; set; }

        public IEnumerable<SelectListItem> LineaComidaLista { get; set; }

        public string Busqueda { get; set; }

        public int? LineaComidaId { get; set; }

        public int PageNumber { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }
    }
}
