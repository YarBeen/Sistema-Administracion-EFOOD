using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.Modelos
{
	public class Rol
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage ="Nombre es Requerido")]
		[MaxLength(40, ErrorMessage ="Nombre debe ser máximo 60 caracteres")]
        public String Nombre { get; set; }
    }
}
