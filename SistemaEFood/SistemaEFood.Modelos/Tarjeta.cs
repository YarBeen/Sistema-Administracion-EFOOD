using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.Modelos
{
    public class Tarjeta
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Nombre es requerido")]
        [MaxLength(40,ErrorMessage ="El nombre es de maximo 40 caracteres")]
        public String Nombre { get; set; }


        public ICollection<ProcesadorDePago> ProcesadoresDePagos { get; set; }

    }
}
