﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaEFood.AccesoDatos.Data;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Modelos;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.AccesoDatos.Repositorio
{
    public class BitacoraErrorRepositorio : Repositorio<BitacoraError>, IBitacoraErrorRepositorio
    {
        private readonly ApplicationDbContext _db;

        public BitacoraErrorRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task RegistrarError(string mensaje, int numeroError)
        {
            var error = new BitacoraError
            {
                Fecha = DateTime.Now,
                Hora = DateTime.Now.ToString("HH:mm:ss"),
                Mensaje = mensaje,
                NumeroError = numeroError
            };
            _db.BitacoraError.Add(error);
            await _db.SaveChangesAsync();
        }
       
        public async Task<IEnumerable<BitacoraError>> ObtenerPorFecha(DateTime fecha)
        {
            return await _db.BitacoraError.Where(x => x.Fecha.Date == fecha.Date).ToListAsync();
        }


    }
}
