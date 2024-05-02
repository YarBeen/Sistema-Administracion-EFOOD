using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaEFood.AccesoDatos.Data;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.AccesoDatos.Repositorio
{
    public class ProcesadorTarjetaRepositorio : Repositorio<ProcesadorTarjeta>, IProcesadorTarjeta
    {
        private readonly ApplicationDbContext _db;

        public ProcesadorTarjetaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(ProcesadorTarjeta procesadorTarjeta)
        {
            var ProcesadorTarjetaBD = _db.ProcesadorTarjeta.FirstOrDefault(b => b.Id == procesadorTarjeta.Id);

            if (ProcesadorTarjetaBD != null)
            {
                _db.SaveChanges();
            }
        }

        public IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj, int? idProcesador)
        {
            if (obj == "Tarjeta")
            {
                // Filtra las tarjetas que no están relacionadas con el idProcesador
                var tarjetas = _db.Tarjetas
                                    .Where(t => !_db.ProcesadorTarjeta
                                                    .Any(pt => pt.TarjetaId == t.Id && pt.ProcesadorId == idProcesador))
                                    .Select(c => new SelectListItem
                                    {
                                        Text = c.Nombre,
                                        Value = c.Id.ToString()
                                    })
                                    .ToList();
                return tarjetas;
            }
            return null;
        }
    }

}
