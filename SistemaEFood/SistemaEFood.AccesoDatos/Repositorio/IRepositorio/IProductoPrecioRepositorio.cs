using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaEFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IProductoPrecioRepositorio : IRepositorio<ProductoPrecio>
    {
        void Actualizar (ProductoPrecio productoPrecio);

        IEnumerable<SelectListItem> ObtenerTipoPrecios(string objeto, int? idProducto);

      
    }
}
