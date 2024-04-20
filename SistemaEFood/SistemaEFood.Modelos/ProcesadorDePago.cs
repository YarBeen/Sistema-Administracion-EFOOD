using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.Modelos
{
    public class ProcesadorDePago
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Nombre es requerido")]
        [MaxLength(40, ErrorMessage = "El nombre es de maximo 40 caracteres")]
        public String Procesador { get; set; }

        [Required(ErrorMessage = "Nombre es requerido")]
        [MaxLength(40, ErrorMessage = "El nombre es de maximo 40 caracteres")]
        public String NombreOpcionDePago { get; set; }

        [Required(ErrorMessage = "Tipo es requerido")]
        [MaxLength(40, ErrorMessage = "El Tipo es de maximo 40 caracteres")]
        public String Tipo { get; set; }

        public bool Estado { get; set; }

        public bool Verificacion { get; set; }

        [Required(ErrorMessage = "Tipo es requerido")]
        [MaxLength(40, ErrorMessage = "El Tipo es de maximo 40 caracteres")]
        public String Metodo { get; set; }


    }
}
