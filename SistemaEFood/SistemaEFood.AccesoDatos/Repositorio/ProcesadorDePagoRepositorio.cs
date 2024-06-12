using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
    public class ProcesadorDePagoRepositorio : Repositorio<ProcesadorDePago>, IProcesadorDePagoRepositorio
    {
        private readonly ApplicationDbContext _db;

        public ProcesadorDePagoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(ProcesadorDePago procesadordepago)
        {
            var procesadordepagoBD = _db.ProcesadorDePago.FirstOrDefault(b => b.Id == procesadordepago.Id);

            if (procesadordepagoBD != null)
            {
                procesadordepagoBD.Procesador = procesadordepago.Procesador;
                procesadordepagoBD.Verificacion = procesadordepago.Verificacion;
                procesadordepagoBD.Estado = procesadordepago.Estado;
                procesadordepagoBD.Metodo = procesadordepago.Metodo;
                procesadordepagoBD.NombreOpcionDePago = procesadordepago.NombreOpcionDePago;
                procesadordepagoBD.Tipo = procesadordepago.Tipo;

                _db.SaveChanges();
            }
        }

        public async Task<IEnumerable<ProcesadorDePago>> ObtenerProcesadorTarjetas()
        {
            var productos = await _db.ProcesadorDePago
             .Where(p => p.Estado == true && p.Tipo.ToLower() == "tarjeta de crédito o débito")
             .ToListAsync();

            return productos;
        }
        public async Task<IEnumerable<ProcesadorDePago>> ObtenerProcesadorCheques()
        {

            var productos = await _db.ProcesadorDePago
             .Where(p => p.Estado == true && p.Tipo.ToLower() == "cheque electrónico")
             .ToListAsync();
            return productos;
        }

        public async Task<IEnumerable<string>> ObtenerTiposDePagoActivos()
        {
            var tipos = await _db.ProcesadorDePago
                .Where(p => p.Estado == true)
                .Select(p => p.Tipo)
                .ToListAsync();
            return tipos;
        }
        public IEnumerable<SelectListItem> ObtenerTodosDropdownLista()
        {
            
                var procesadoresTarjetaCreditoDebito = _db.ProcesadorDePago
                                                          .Where(p => (p.Tipo == "tarjeta de crédito o débito") && p.Estado)
                                                          .Select(p => p.Id)
                                                          .ToList();

                var tarjetasRelacionadas = _db.ProcesadorTarjeta
                                               .Where(pt => procesadoresTarjetaCreditoDebito.Contains(pt.ProcesadorId))
                                               .Select(pt => pt.TarjetaId)
                                               .Distinct()
                                               .ToList();

                var tarjetasInfo = _db.Tarjetas
                                      .Where(t => tarjetasRelacionadas.Contains(t.Id))
                                      .Select(t => new SelectListItem
                                      {
                                          Text = t.Nombre,
                                          Value = t.Id.ToString()
                                      })
                                      .ToList();

                return tarjetasInfo;
            
            
        }


    }
}
