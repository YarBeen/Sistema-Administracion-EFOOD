using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.Modelos.ViewModels
{
    public class UsuarioVM
    {
        public string Prueba {  get; set; }
        public Usuario UserModel { get; set; }

        public IEnumerable<SelectListItem> ListaRoles { get; set; }

        
    }
}
