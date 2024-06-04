using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.Modelos
{
    public class CarroCompra
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }

        public string Cliente { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public double Precio { get; set; }

        [Required]
        public string TipoDePrecio { get; set; }



    }
}
