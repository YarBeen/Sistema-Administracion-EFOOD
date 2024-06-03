using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.Modelos
{
    public class Orden
    {
        [Key]
        public int Id { get; set; }

        public DateTime FechaOrden { get; set; }

        public string NumeroEnvio { get; set; }

        [Required]
        public double TotalOrden { get; set; }

        [Required]
        public string EstadoOrden { get; set; }

        public string Telefono { get; set; }

        public string Direccion { get; set; }

        public string NombresCliente { get; set; }

        public string ApellidosCliente { get; set; }

        public string CodigoTiqueteDeDescuento { get; set; }

    }
}
