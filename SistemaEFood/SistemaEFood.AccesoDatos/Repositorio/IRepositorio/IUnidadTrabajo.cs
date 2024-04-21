using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SistemaEFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IUnidadTrabajo : IDisposable
    {
        ITarjetaRepositorio Tarjeta { get; }
        IProcesadorDePagoRepositorio ProcesadorDePago { get; }
        IUsuarioRepositorio Usuario {  get; }

        ITiqueteDeDescuentoRepositorio TiqueteDeDescuento { get; }
        ILineaComidaRepositorio LineaComida {  get; }
        Task Guardar();
    }
}