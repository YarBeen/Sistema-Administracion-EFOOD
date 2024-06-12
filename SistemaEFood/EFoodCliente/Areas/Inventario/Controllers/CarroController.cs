﻿using Microsoft.AspNetCore.Mvc;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Modelos;
using SistemaEFood.Modelos.ViewModels;
using SistemaEFood.Utilidades;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EFoodCliente.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class CarroController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        [BindProperty]
        public CarroCompraVM carroCompraVM { get; set; }

        public CarroController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public async Task<IActionResult> Index()
        {
            string usuarioId = Request.Cookies["UsuarioId"];

            carroCompraVM = new CarroCompraVM();
            carroCompraVM.Orden = new Orden();
            carroCompraVM.CarroCompraLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(u => u.Cliente == usuarioId, incluirPropiedades: "Producto");

            carroCompraVM.Orden.TotalOrden = 0;

            foreach (var lista in carroCompraVM.CarroCompraLista)
            {
                carroCompraVM.Orden.TotalOrden += ((lista.Precio) * (lista.Cantidad));
            }

            return View(carroCompraVM);
        }

        public async Task<IActionResult> mas(int carroId)
        {
            var carroCompras = await _unidadTrabajo.CarroCompra.ObtenerPrimero(c => c.Id == carroId);
            carroCompras.Cantidad += 1;
            await _unidadTrabajo.Guardar();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> menos(int carroId)
        {
            var carroCompras = await _unidadTrabajo.CarroCompra.ObtenerPrimero(c => c.Id == carroId);
            string usuarioId = Request.Cookies["UsuarioId"];
           

            if (carroCompras.Cantidad == 1)
            {
                var carroLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(c => c.Cliente == usuarioId);
                var numeroProductos = carroLista.Count();
                _unidadTrabajo.CarroCompra.Remover(carroCompras);
                await _unidadTrabajo.Guardar();
                HttpContext.Session.SetInt32(DS.SesionCarroCompras, numeroProductos - 1);
            }
            else
            {
                carroCompras.Cantidad -= 1;
                await _unidadTrabajo.Guardar();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> remover(int carroId)
        {
            var carroCompras = await _unidadTrabajo.CarroCompra.ObtenerPrimero(c => c.Id == carroId);
            string usuarioId = Request.Cookies["UsuarioId"];
            var carroLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(c => c.Cliente == usuarioId);
            var numeroProductos = carroLista.Count();
            _unidadTrabajo.CarroCompra.Remover(carroCompras);
            await _unidadTrabajo.Guardar();
            HttpContext.Session.SetInt32(DS.SesionCarroCompras, numeroProductos - 1);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Proceder()
        {
            string usuarioId = Request.Cookies["UsuarioId"];

            carroCompraVM = new CarroCompraVM()
            {
                Orden = new Orden(),
                CarroCompraLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(u => u.Cliente == usuarioId, incluirPropiedades: "Producto")
            };

            foreach (var lista in carroCompraVM.CarroCompraLista)
            {
                carroCompraVM.Orden.TotalOrden += ((lista.Precio) * (lista.Cantidad));
            }
            carroCompraVM.Orden.FechaOrden = DateTime.Now;

            return View(carroCompraVM);
        }

        [HttpPost]
        public async Task<IActionResult> Proceder(CarroCompraVM carroCompraVM)
        {
            string usuarioId = Request.Cookies["UsuarioId"];

            carroCompraVM.CarroCompraLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(c => c.Cliente == usuarioId, incluirPropiedades: "Producto");
            carroCompraVM.Orden.TotalOrden = 0;
            carroCompraVM.Orden.Cliente = usuarioId;
            carroCompraVM.Orden.FechaOrden = DateTime.Now;

            foreach (var lista in carroCompraVM.CarroCompraLista)
            {
                carroCompraVM.Orden.TotalOrden += ((lista.Precio) * (lista.Cantidad));
            }

            carroCompraVM.Orden.EstadoOrden = "En curso";

            await _unidadTrabajo.Orden.Agregar(carroCompraVM.Orden);
            await _unidadTrabajo.Guardar();

            

            return RedirectToAction("SeleccionarMetodo");
        }

        public async Task<IActionResult> SeleccionarMetodo()
        {
            string usuarioId = Request.Cookies["UsuarioId"];
            var ordenes = await _unidadTrabajo.Orden.ObtenerTodos(u => u.Cliente == usuarioId);
            var orden = ordenes.OrderByDescending(u => u.Id).FirstOrDefault();
            CarroCompraVM carroCompraVM = new CarroCompraVM()
            {
                Orden = orden,
                CarroCompraLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(u => u.Cliente == usuarioId, incluirPropiedades: "Producto")
            };

            var tiquete = await _unidadTrabajo.TiqueteDeDescuento.ObtenerPrimero(t => t.Codigo == carroCompraVM.Orden.CodigoTiqueteDeDescuento);

            if (tiquete != null)
            {
                carroCompraVM.Orden.Descuento = tiquete.Descuento;
            }
            else
            {
                carroCompraVM.Orden.Descuento = 0;
            }

            carroCompraVM.ListaPagosActivo = await _unidadTrabajo.ProcesadorDePago.ObtenerTiposDePagoActivos();

            return View(carroCompraVM);
        }
        

       
        public async Task<IActionResult> ConfirmacionFinal(string MetodoDePago,string Tipo)
        {
            string usuarioId = Request.Cookies["UsuarioId"];
            var ordenes = await _unidadTrabajo.Orden.ObtenerTodos(u => u.Cliente == usuarioId);
            var orden = ordenes.OrderByDescending(u => u.Id).FirstOrDefault();
            CarroCompraVM carroCompraVM = new CarroCompraVM()
            {
                Orden = orden,
                CarroCompraLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(u => u.Cliente == usuarioId, incluirPropiedades: "Producto")
            };

            var tiquete = await _unidadTrabajo.TiqueteDeDescuento.ObtenerPrimero(t => t.Codigo == carroCompraVM.Orden.CodigoTiqueteDeDescuento);

            if (tiquete != null)
            {
                carroCompraVM.Orden.Descuento = tiquete.Descuento;
            }
            else
            {
                carroCompraVM.Orden.Descuento = 0;
            }


            carroCompraVM.OrdenDetalle = new OrdenDetalle()
            {
                
                Orden = orden,
                OrdenId = carroCompraVM.Orden.Id,
                Tipo = MetodoDePago,
                Medio = Tipo,
                Monto = orden.TotalOrden-(carroCompraVM.Orden.Descuento * orden.TotalOrden / 100)   
            };

            return View(carroCompraVM);
        }

        [HttpPost]
        public async Task<IActionResult> GuardarPedido(CarroCompraVM carroCompraVM)
        {
            carroCompraVM.OrdenDetalle.Estado = "En Curso";
            carroCompraVM.OrdenDetalle.FechaOrden = DateTime.Now;

            _unidadTrabajo.OrdenDetalle.Agregar(carroCompraVM.OrdenDetalle);
            await _unidadTrabajo.Guardar();

            string usuarioId = Request.Cookies["UsuarioId"];
            var carroLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(c => c.Cliente == usuarioId);
            foreach (var item in carroLista)
            {
                _unidadTrabajo.CarroCompra.Remover(item);
                await _unidadTrabajo.Guardar();
            }



            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [HttpPost]
        public async Task<IActionResult> CancelarPedido(CarroCompraVM carroCompraVM)
        {
            carroCompraVM.OrdenDetalle.Estado = "Cancelado";
            carroCompraVM.OrdenDetalle.FechaOrden = DateTime.Now;

            _unidadTrabajo.OrdenDetalle.Agregar(carroCompraVM.OrdenDetalle);
            await _unidadTrabajo.Guardar();
            string usuarioId = Request.Cookies["UsuarioId"];
            var carroLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(c => c.Cliente == usuarioId);
            foreach (var item in carroLista)
            {
                _unidadTrabajo.CarroCompra.Remover(item);
                await _unidadTrabajo.Guardar();
            }

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [HttpGet]
        public IActionResult InformacionDeTarjeta(CarroCompraVM carroCompraVM)
        {
            carroCompraVM.TarjetaLista = _unidadTrabajo.ProcesadorTarjeta.ObtenerTodosDropdownLista("Tarjeta", -1);
;
            return View(carroCompraVM);
        }

        [HttpPost]
        public async Task<IActionResult> ProcesarPagoConTarjeta(string tipoTarjeta, string numeroTarjeta, int mesExpiracion, int anoExpiracion, string cvv, CarroCompraVM carroCompraVM)
        {
            
            return RedirectToAction("ConfirmacionFinal", new { MetodoDePago = tipoTarjeta, Tipo = "Tarjeta de Crédito o Débito" });
        }

        [HttpPost]
        public async Task<IActionResult> SeleccionarMetodoPost(string action, CarroCompraVM carroCompraVM, string metodoPago)
        {
            if (action == "anterior")
            {
                return RedirectToAction("Proceder");
            }
            else if (action == "siguiente")
            {
                if (metodoPago == "Efectivo")
                {
                    return RedirectToAction("ConfirmacionFinal", new { MetodoDePago = metodoPago, Tipo = "Efectivo" });
                }
                else 
                {
                    return RedirectToAction("InformacionDeTarjeta", carroCompraVM);
                }

            }

            return View(carroCompraVM);
        }

        [HttpPost]
        public IActionResult VolverAMetodoPago(CarroCompraVM carroCompraVM)
        {
            return RedirectToAction("SeleccionarMetodo"); 
        }

    }
}
