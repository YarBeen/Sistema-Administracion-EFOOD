using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.Modelos
{
    public class OrdenDetalle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrdenId { get; set; }

        [ForeignKey("OrdenId")]
        public Orden Orden { get; set; }

        public string Medio { get; set; }

        public string Tipo { get; set; }

        public int ChequeNumero { get; set; }

        public int ChequeCuenta{ get; set; }

        [Required]
        public string Estado { get; set; }


        [Required]
        public double Monto { get; set; }

        public DateTime FechaOrden { get; set; }
    }

}
