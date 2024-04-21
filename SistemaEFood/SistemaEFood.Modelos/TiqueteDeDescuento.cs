using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.Modelos
{
    public class TiqueteDeDescuento
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Codigo es requerido")]
        [MaxLength(40, ErrorMessage = "El codigo es de maximo 40 caracteres")]
        public String Codigo { get; set; }

        [Required(ErrorMessage = "Nombre es requerido")]
        [MaxLength(40, ErrorMessage = "El nombre es de maximo 40 caracteres")]
        public String Nombre { get; set; }

        [Required(ErrorMessage = "Cantidad de Disponibles es requerido")]

        public int Disponibles { get; set; }
        [Required(ErrorMessage = "Cantidad de Descuento es requerido")]
        public int Descuento { get; set; }
    }
}
