using Microsoft.AspNetCore.Mvc;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Modelos;
using SistemaEFood.Modelos.ViewModels;
using SistemaEFood.Utilidades;
using System.Diagnostics;

namespace SistemaEFood.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnidadTrabajo _unidadTrabajo;
        [BindProperty]
        private CarroCompraVM carroCompraVM { get; set; }
        public HomeController(ILogger<HomeController> logger, IUnidadTrabajo unidadTrabajo)
        {
            _logger = logger;
            _unidadTrabajo = unidadTrabajo;
        }

        public async Task<IActionResult> Index()
        {

            if (!Request.Cookies.TryGetValue("UsuarioId", out string usuarioId))
            {
                usuarioId = Guid.NewGuid().ToString();

                Response.Cookies.Append("UsuarioId", usuarioId);
            }

            ProductoVM productoVM = new ProductoVM()
            {
                Producto = new Producto(),
                LineaComidaLista = _unidadTrabajo.Producto.ObtenerTodosDropdownLista("LineaComida"),
                ProductosLista = await _unidadTrabajo.Producto.ObtenerTodos()
            };
            return View(productoVM);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var carroCompraVM = new CarroCompraVM();
            carroCompraVM.Producto = await _unidadTrabajo.Producto.ObtenerPrimero(p => p.Id == id);

            carroCompraVM.CarroCompra = new CarroCompra()
            {
                Producto = carroCompraVM.Producto,
                ProductoId = carroCompraVM.Producto.Id
            };

            carroCompraVM.ListaPrecios = (await _unidadTrabajo.Producto.ObtenerPreciosPorTamanno(id)).ToList();

            return View(carroCompraVM);
        }

        [HttpPost]
        public async Task<IActionResult> Detalle(CarroCompraVM carroCompraVM)
        {


            if (Request.Cookies.TryGetValue("UsuarioId", out string usuarioId))
            {
                carroCompraVM.CarroCompra.Cliente = usuarioId;

                CarroCompra carroBD = await _unidadTrabajo.CarroCompra.ObtenerPrimero(c => c.Cliente == usuarioId &&
                                                                                          c.ProductoId == carroCompraVM.CarroCompra.ProductoId);
                if (carroBD == null)
                {
                    await _unidadTrabajo.CarroCompra.Agregar(carroCompraVM.CarroCompra);
                }
                else
                {
                    carroBD.Cantidad += carroCompraVM.CarroCompra.Cantidad;
                    _unidadTrabajo.CarroCompra.Actualizar(carroBD);
                }
                await _unidadTrabajo.Guardar();
                TempData[DS.Exitosa] = "Producto agregado al Carro de Compras";
                TempData[DS.Exitosa] = "Producto agregado al Carro de Compras";

            }

            return RedirectToAction("Index");
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
