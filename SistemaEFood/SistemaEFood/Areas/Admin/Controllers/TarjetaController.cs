using Humanizer;
using Microsoft.AspNetCore.Mvc;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Modelos;
using SistemaEFood.Utilidades;

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


        public async Task<IActionResult> Upsert(int? id)
        {
            Tarjeta tarjeta = new Tarjeta();

            if(id == null)
            {
                return View(tarjeta);
            }
            tarjeta = await _unidadTrabajo.Tarjeta.Obtener(id.GetValueOrDefault());
            if(tarjeta == null)
            {
                return NotFound();
            }
            return View(tarjeta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Tarjeta tarjeta)
        {
            if (ModelState.IsValid)
            {
                if (tarjeta.Id == 0)
                {
                    await _unidadTrabajo.Tarjeta.Agregar(tarjeta);
                    TempData[DS.Exitosa] = "Tarjeta creada exitosamente";
                }
                else
                {
                    _unidadTrabajo.Tarjeta.Actualizar(tarjeta);
                    TempData[DS.Exitosa] = "Tarjeta actualizada exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al grabar tarjeta";
            return View(tarjeta);
        }


        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Tarjeta.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var tarjetaDb = await _unidadTrabajo.Tarjeta.Obtener(id);
            if (tarjetaDb == null)
            {
                return Json(new { success = false, message = "Error al borrar Tarjeta" });
            }
            _unidadTrabajo.Tarjeta.Remover(tarjetaDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Tarjeta borrada exitosamente" });
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Tarjeta.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim() && b.Id != id);
            }
            if (valor)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });
        }

        #endregion

    }
}
