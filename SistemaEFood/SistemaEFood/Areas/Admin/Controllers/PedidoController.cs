using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Modelos;
using SistemaEFood.Modelos.ViewModels;
using SistemaEFood.Utilidades;
using SistemaEFood.Servicios;
using Microsoft.IdentityModel.Tokens;

namespace SistemaEFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Mantenimiento)]
    
    public class PedidoController : Controller
    {
       
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly IStorageService _storageService;

        public PedidoController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment webHostEnviroment, IStorageService storageService)
        {
            
            _unidadTrabajo = unidadTrabajo;
            _webHostEnviroment = webHostEnviroment;
            _storageService = storageService;
        }
        public IActionResult Index()
        {
            return View();
        }

        
        public async Task<IActionResult> Consultar()
        {
            PedidoVM pedidoVM = new PedidoVM()
            {
                ListaOrdenDetalles = await _unidadTrabajo.OrdenDetalle.ObtenerTodos()
            };
            return View(pedidoVM);
        }




        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.OrdenDetalle.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorFecha(DateTime fecha)
        {
            if (fecha == DateTime.MinValue)
            {
                var todos = await ObtenerTodos();
                return todos;
            }
            var registrosBitacora = await _unidadTrabajo.OrdenDetalle.ObtenerPorFecha(fecha);
            return Json(new { data = registrosBitacora });

        }

        [HttpGet]
        public async Task<IActionResult> ConsultarConFiltro(DateTime fechainicial, DateTime fechafinal, string estado)
        {
            IEnumerable<OrdenDetalle> registrosOrdenes;

            if (fechainicial != DateTime.MinValue && fechafinal != DateTime.MinValue)
            {
                registrosOrdenes = await _unidadTrabajo.OrdenDetalle.ObtenerEntreFechas(fechainicial, fechafinal);

                if (!string.IsNullOrEmpty(estado) && estado != "-- Seleccione un estado --")
                {
                    registrosOrdenes = registrosOrdenes.Where(p => p.Estado == estado);
                }
            }
            else
            {
                registrosOrdenes = await _unidadTrabajo.OrdenDetalle.ObtenerTodos();

                if (!string.IsNullOrEmpty(estado) && estado != "-- Seleccione un estado --")
                {
                    registrosOrdenes = registrosOrdenes.Where(p => p.Estado == estado);
                }
            }

            return Json(new { data = registrosOrdenes });
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorEstado(string estado)
        {
            if (string.IsNullOrEmpty(estado))
            {
                return await ObtenerTodos();
            }

            var pedidos = await _unidadTrabajo.OrdenDetalle.ObtenerPedidosPorEstado(estado);
            return Json(new { data = pedidos });
        }


        [HttpPost]
        public async Task<IActionResult> EliminarPedidos(List<int> ids)
        {
            var usuarioNombre = User.Identity.Name;
            try
            {
                if (ids == null || ids.Count == 0)
                {
                    return Json(new { success = false, message = "La lista de IDs está vacía." });
                }
                var numPedidos = "";
                foreach (var id in ids)
                {
                    var pedido = await _unidadTrabajo.OrdenDetalle.ObtenerPrimero(p => p.Id == id);
                    if (pedido != null)
                    {
                        _unidadTrabajo.OrdenDetalle.Remover(pedido);
                        numPedidos += pedido.Id + ", ";
                    }
                }
                var mensaje = "Se eliminó los pedidos: " + numPedidos;
                await _unidadTrabajo.Bitacora.RegistrarAccion(usuarioNombre, mensaje.ToString());

                await _unidadTrabajo.Guardar();
                return Json(new { success = true});
            }
            catch (Exception ex)
            {
                var mensajeError = "Error al intentar borrar pedidos";
                await _unidadTrabajo.BitacoraError.RegistrarError(mensajeError.ToString(), 400);
                return Json(new { success = false});
            }
        }


        #endregion

    }
}
