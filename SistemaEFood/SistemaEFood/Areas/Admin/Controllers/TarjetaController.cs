using Humanizer;
using Microsoft.AspNetCore.Mvc;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;

namespace SistemaEFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TarjetaController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public TarjetaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Tarjeta.ObtenerTodos();
            return Json(new { data = todos });
        }
        #endregion

    }
}
