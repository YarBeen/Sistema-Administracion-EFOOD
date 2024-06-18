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
    public class LineaComidaController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public LineaComidaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }
        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Upsert(int? id)
        {
            LineaComida lineaComida = new LineaComida();

            if(id == null)
            {
                return View(lineaComida);
            }
			lineaComida = await _unidadTrabajo.LineaComida.Obtener(id.GetValueOrDefault());
            if(lineaComida == null)
            {
                return NotFound();
            }
            return View(lineaComida);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(LineaComida lineaComida)
        {
            var usuarioNombre = User.Identity.Name;
            if (ModelState.IsValid)
            {
                if (lineaComida.Id == 0)
                {
                    await _unidadTrabajo.LineaComida.Agregar(lineaComida);
                    var mensaje = TempData[DS.Exitosa] = "Se creó una linea de Comida con nombre " + lineaComida.Nombre + " exitosamente";
                    await _unidadTrabajo.Bitacora.RegistrarAccion(usuarioNombre,mensaje.ToString());
                    
                }
                else
                {
                    _unidadTrabajo.LineaComida.Actualizar(lineaComida);
                    var mensaje = TempData[DS.Exitosa] = "Se actualizó una linea de Comida con nombre " + lineaComida.Nombre + " exitosamente";
                    await _unidadTrabajo.Bitacora.RegistrarAccion(usuarioNombre, mensaje.ToString());
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            var mensajeError = TempData[DS.Error] = "Error al grabar linea de comida";
            await _unidadTrabajo.BitacoraError.RegistrarError(mensajeError.ToString(), 400);
            return View(lineaComida);
        }




        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.LineaComida.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioNombre = User.Identity.Name;
            var lineaComidaDb = await _unidadTrabajo.LineaComida.Obtener(id);
            if (lineaComidaDb == null)
            {
                var mensajeError = "Error al borrar una linea de comida";
                await _unidadTrabajo.BitacoraError.RegistrarError(mensajeError, 300);
                return Json(new { success = false, message = "Error al borrar linea de comida" });
            }

            _unidadTrabajo.LineaComida.Remover(lineaComidaDb);
            await _unidadTrabajo.Guardar();
            var mensaje = "Se ha borrado la linea de comida " + lineaComidaDb.Nombre + " exitosamente";
            await _unidadTrabajo.Bitacora.RegistrarAccion(usuarioNombre, mensaje);
            return Json(new { success = true, message = "Linea de comida borrada exitosamente" });
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            if(nombre== null)
            {
                return Json(new { data = false });

            }
            bool valor = false;
            var lista = await _unidadTrabajo.LineaComida.ObtenerTodos();
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
