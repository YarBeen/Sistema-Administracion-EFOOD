﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.Modelos.ViewModels
{
    public class CarroCompraVM
    {
        public Producto Producto { get; set; }

        public CarroCompra CarroCompra { get; set; }

        public List<(string TipoPrecioNombre, float Monto)> ListaPrecios { get; set; } 

        public IEnumerable<CarroCompra> CarroCompraLista { get; set; }

        public IEnumerable<string> ListaPagosActivo { get; set; }

        public IEnumerable<SelectListItem> TarjetaLista { get; set; }

        public Orden Orden { get; set; }

        public OrdenDetalle OrdenDetalle { get; set; }

        public string TipoTarjeta { get; set; }

        public string NumeroCheque { get; set; }
        public string Cuenta { get; set; }

    }
}
