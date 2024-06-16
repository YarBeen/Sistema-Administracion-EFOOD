using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Modelos;
using SistemaEFood.Utilidades;

namespace SistemaEFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Mantenimiento)]
    public class TipoPrecioController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public TipoPrecioController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }
        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Upsert(int? id)
        {
            TipoPrecio tipoPrecio = new TipoPrecio();

            if(id == null)
            {
                return View(tipoPrecio);
            }
            tipoPrecio = await _unidadTrabajo.TipoPrecio.Obtener(id.GetValueOrDefault());
            if(tipoPrecio == null)
            {
                return NotFound();
            }
            return View(tipoPrecio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TipoPrecio tipoPrecio)
        {
            var usuarioNombre = User.Identity.Name;
            if (ModelState.IsValid)
            {
                if (tipoPrecio.Id == 0)
                {
                    await _unidadTrabajo.TipoPrecio.Agregar(tipoPrecio);
                    TempData[DS.Exitosa] = "Tipo de Precio creada exitosamente";
                    await _unidadTrabajo.Bitacora.RegistrarAccion(usuarioNombre, "Se creó el tipo de precio " + tipoPrecio.Nombre + " de forma exitosa");
                }
                else
                {
                    _unidadTrabajo.TipoPrecio.Actualizar(tipoPrecio);
                    TempData[DS.Exitosa] = "Tipo de Precio actualizado exitosamente";
                    await _unidadTrabajo.Bitacora.RegistrarAccion(usuarioNombre, "Se actualizó el tipo de precio " + tipoPrecio.Nombre + " de forma exitosa");
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            var mensaje = TempData[DS.Error] = "Error al grabar tipo de precio";
            await _unidadTrabajo.BitacoraError.RegistrarError(mensaje.ToString(), 400);
            return View(tipoPrecio);
        }




        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.TipoPrecio.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioNombre = User.Identity.Name;
            var tipoPrecioDb = await _unidadTrabajo.TipoPrecio.Obtener(id);
            if (tipoPrecioDb == null)
            {
                var mensaje = TempData[DS.Error] = "Error al remover tipo de precio";
                await _unidadTrabajo.BitacoraError.RegistrarError(mensaje.ToString(), 400);
                return Json(new { success = false, message = "Error al borrar Tipo de Precio" });
            }

            _unidadTrabajo.TipoPrecio.Remover(tipoPrecioDb);
            await _unidadTrabajo.Guardar();
            await _unidadTrabajo.Bitacora.RegistrarAccion(usuarioNombre,"Se borro " + tipoPrecioDb.Nombre + " de forma exitosa");
            return Json(new { success = true, message = "Tipo de precio borrado exitosamente" });
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.TipoPrecio.ObtenerTodos();
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
