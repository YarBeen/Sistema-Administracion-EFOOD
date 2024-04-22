using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.Modelos
{
    public class TipoPrecio
    {
        [Key]
        public int Id { get; set; }

      
        [Required(ErrorMessage = "Nombre es requerido")]
        [MaxLength(40, ErrorMessage = "El nombre es de maximo 40 caracteres")]
        public string Nombre { get; set; }
    }
}
