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
    }
}
