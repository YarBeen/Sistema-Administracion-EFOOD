using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.Modelos
{
    public class ProductoPrecio
    {
        [Key]
        public int Id { get; set; }


        public int Idprecio { get; set; }

        [ForeignKey("Idprecio")]
        public TipoPrecio TipoPrecio { get; set; }

        public int Idproducto { get; set; }

        [ForeignKey("Idproducto")]
        public Producto Producto { get; set; }

        public float Monto { get; set; }

        [NotMapped] //No se agrega a la bd
        public string? Tipo { get; set; }
    }
}
