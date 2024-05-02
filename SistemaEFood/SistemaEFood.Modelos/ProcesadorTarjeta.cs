using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.Modelos
{
    public class ProcesadorTarjeta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProcesadorId { get; set; }
        [ForeignKey("ProcesadorId")]
        public ProcesadorDePago ProcesadorDePago { get; set; }

        public int TarjetaId{ get; set; }
      
        
        [ForeignKey("TarjetaId")]
        public Tarjeta Tarjeta { get; set; }
    }
}
