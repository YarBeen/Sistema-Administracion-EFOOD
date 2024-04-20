﻿using SistemaEFood.AccesoDatos.Data;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.AccesoDatos.Repositorio
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        private readonly ApplicationDbContext _db;
        public ITarjetaRepositorio Tarjeta { get; private set; }
        public IProcesadorDePagoRepositorio ProcesadorDePago { get; private set; }
        public UnidadTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Tarjeta = new TarjetaRepositorio(_db);
            ProcesadorDePago = new ProcesadorDePagoRepositorio(_db);   
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task Guardar()
        {
           await _db.SaveChangesAsync();
        }
    }
}
