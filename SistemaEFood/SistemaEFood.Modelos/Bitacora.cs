using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.Modelos
{
    public class Bitacora
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        [MaxLength(8)]
        public string Hora { get; set; }

        [Required]
        [MaxLength(250)]
        public string Mensaje { get; set; }

        public string nombreUsuario { get; set; }
    }
}
