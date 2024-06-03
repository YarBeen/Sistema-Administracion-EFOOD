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
        ITipoPrecioRepositorio TipoPrecio { get; }

        IProductoRepositorio Producto { get; }

        IProcesadorTarjeta ProcesadorTarjeta { get; }

        IProductoPrecioRepositorio ProductoPrecio { get; }

        IBitacoraErrorRepositorio BitacoraError { get; }

        ICarroCompraRepositorio CarroCompra {  get; }

        IOrdenRepositorio Orden { get; }

        IOrdenDetalleRepositorio OrdenDetalle { get; }
        Task Guardar();
    }
}