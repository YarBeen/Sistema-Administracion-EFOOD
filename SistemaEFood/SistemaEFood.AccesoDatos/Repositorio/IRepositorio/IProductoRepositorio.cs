using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaEFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IProductoRepositorio : IRepositorio<Producto>
    {
        void Actualizar(Producto producto);

        IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj);


        public Task<IEnumerable<Producto>> ObtenerTodosPorLineaComida(int idLineaComida);

    }
}
