using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.Modelos
{
    public class Usuario : IdentityUser
    {
        

        [Required(ErrorMessage = "Pregunta de Seguridad es Requerida")]
        [MaxLength(60)]
        public string PreguntaSeguridad { get; set; }


        [Required(ErrorMessage = "Respuesta a Pregunta de Seguridad es Requerida")]
        [MaxLength(50)]
        public string RespuestaSeguridad { get; set; }


		[Required(ErrorMessage = "Estado es Requerido")]
		public bool Estado { get; set; }


        [NotMapped]
        
        public string Role { get; set; }

    }
}
