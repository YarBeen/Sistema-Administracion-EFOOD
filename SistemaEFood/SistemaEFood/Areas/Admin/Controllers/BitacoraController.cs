using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Modelos;
using SistemaEFood.Modelos.ViewModels;
using SistemaEFood.Utilidades;

namespace SistemaEFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Mantenimiento)]
    public class BitacoraController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public BitacoraController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment webHostEnviroment)
        {
            _unidadTrabajo = unidadTrabajo;
            _webHostEnviroment = webHostEnviroment;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Bitacora.ObtenerTodos();
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
            var registrosBitacora = await _unidadTrabajo.Bitacora.ObtenerPorFecha(fecha);
            return Json(new { data = registrosBitacora });
       
        }
        #endregion

    }
}
