using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaEFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IUsuarioRepositorio : IRepositorio<Usuario>
    {

        void Actualizar(Usuario usuario);
        Task<Usuario> ObtenerUsuarioPorID(string id);

        IEnumerable<SelectListItem> ObtenerRoles();
        Task<bool> ActualizarPasswordAsync(string userId, string newPassword);
    }
}
