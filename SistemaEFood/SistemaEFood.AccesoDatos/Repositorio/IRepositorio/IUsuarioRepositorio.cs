using Microsoft.AspNetCore.Mvc;
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
        IActionResult ObtenerUsuarioPorID(string id);
    }
}
