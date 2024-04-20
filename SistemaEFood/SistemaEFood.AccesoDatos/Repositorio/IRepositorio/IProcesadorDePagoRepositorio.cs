﻿using SistemaEFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IProcesadorDePagoRepositorio: IRepositorio<ProcesadorDePago>
    {
        void Actualizar(ProcesadorDePago procesadordepago);

    }
}
