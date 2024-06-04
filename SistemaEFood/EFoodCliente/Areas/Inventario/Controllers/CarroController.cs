using Microsoft.AspNetCore.Mvc;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Modelos.ViewModels;
using SistemaEFood.Utilidades;

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
            carroCompraVM.Orden = new SistemaEFood.Modelos.Orden();
            carroCompraVM.CarroCompraLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(u => u.Cliente == usuarioId,incluirPropiedades: "Producto");

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
                var carroLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(
                                                c => c.Cliente == usuarioId);
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
            var carroLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(
                                                c => c.Cliente == usuarioId);
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
                Orden = new SistemaEFood.Modelos.Orden(),
                CarroCompraLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(u => u.Cliente == usuarioId, incluirPropiedades: "Producto")
                
            };

            foreach (var lista in carroCompraVM.CarroCompraLista)
            {
                carroCompraVM.Orden.TotalOrden += ((lista.Precio) * (lista.Cantidad));
            }
            carroCompraVM.Orden.NombresCliente = "";
            carroCompraVM.Orden.ApellidosCliente = "";

            carroCompraVM.Orden.Telefono = "";
            carroCompraVM.Orden.Direccion = "";
            carroCompraVM.Orden.CodigoTiqueteDeDescuento = "";
            carroCompraVM.Orden.FechaOrden =  DateTime.Now;


            return View(carroCompraVM);

        }
    }
}
