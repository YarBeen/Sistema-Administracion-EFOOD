using Microsoft.AspNetCore.Mvc;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Modelos;
using SistemaEFood.Modelos.Especificaciones;
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

        public async Task<IActionResult> Index(int pageNumber = 1, string busqueda="", string busquedaActual="", int? idLineaComida = null)
        {

            string usuarioId;

            if (!Request.Cookies.TryGetValue("UsuarioId", out usuarioId))
            {
                usuarioId = Guid.NewGuid().ToString();

                Response.Cookies.Append("UsuarioId", usuarioId);
            }
            var carroLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(c => c.Cliente == usuarioId);
            var numeroProductos = carroLista.Count();
            HttpContext.Session.SetInt32(DS.SesionCarroCompras, numeroProductos);


            if (!String.IsNullOrEmpty(busqueda))
            {
                pageNumber = 1;
            }
            else
            {
                busqueda = busquedaActual;
            }
            ViewData["BusquedaActual"] = busqueda;
            ViewData["LineaComidaActual"] = idLineaComida;

            if (pageNumber < 1) { pageNumber = 1; }
            Parametros parametros = new Parametros()
            {
                PageNumber = pageNumber,
                PageSize = 8
            };

            var resultado = _unidadTrabajo.Producto.ObtenerTodosPaginado(parametros);

            if(!String.IsNullOrEmpty(busqueda))
            {
                resultado = _unidadTrabajo.Producto.ObtenerTodosPaginado(parametros, p => p.Contenido.Contains(busqueda));
            }

            if (idLineaComida.HasValue)
            {
                resultado = _unidadTrabajo.Producto.ObtenerTodosPaginado(parametros, 
                    p => p.LineaComidaId == idLineaComida.Value && (string.IsNullOrEmpty(busqueda)
                    || p.Contenido.Contains(busqueda)));
            }
            
            ViewData["TotalPaginas"] = resultado.MetaData.TotalPages;
            ViewData["TotalRegistros"] = resultado.MetaData.TotalCount;
            ViewData["PageSize"] = resultado.MetaData.PageSize;
            ViewData["PageNumber"] = pageNumber;
            ViewData["Previo"] = "disabled";
            ViewData["Siguiente"] = "";

            if (pageNumber > 1) { ViewData["Previo"] = ""; }
            if (resultado.MetaData.TotalPages <= pageNumber) { ViewData["Siguiente"] = "disabled"; }


            var productoBusquedaVM = new ProductoBusquedaVM()
            {
                Productos = resultado,
                LineaComidaLista = _unidadTrabajo.Producto.ObtenerTodosDropdownLista("LineaComida"),
                Busqueda = busqueda,
                LineaComidaId = idLineaComida,
                PageNumber = pageNumber,
                TotalPages = resultado.MetaData.TotalPages,
                PageSize = resultado.MetaData.PageSize
            };

            ProductoVM productoVM = new ProductoVM()
            {
                Producto = new Producto(),
                LineaComidaLista = _unidadTrabajo.Producto.ObtenerTodosDropdownLista("LineaComida"),
                ProductosLista = await _unidadTrabajo.Producto.ObtenerTodos()
            };

            return View(productoBusquedaVM);

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
             
                await _unidadTrabajo.CarroCompra.Agregar(carroCompraVM.CarroCompra);
                
              
                await _unidadTrabajo.Guardar();
                

                var carroLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(c => c.Cliente == usuarioId);
                var numeroProductos = carroLista.Count();
                HttpContext.Session.SetInt32(DS.SesionCarroCompras, numeroProductos);
                TempData[DS.Exitosa] = "Producto agregado al Carro de Compras";
                return RedirectToAction("Index");

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
