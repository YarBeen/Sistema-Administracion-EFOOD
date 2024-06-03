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
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Nombre del producto es requerido")]
        [MaxLength(40)]
        public string Nombre { get; set; }

        [Required(ErrorMessage ="Contenido del producto es requerido")]
        [MaxLength(100)]
        public string Contenido { get; set; }

        public string ImagenUrl { get; set; }

        [Required(ErrorMessage ="Linea de comida es requerida")]
        public int LineaComidaId { get; set; }


        [ForeignKey("LineaComidaId")]
        public LineaComida LineaComida { get; set; }


        public int? PadreId { get; set; }
        public virtual Producto Padre { get; set; }

    }
}
