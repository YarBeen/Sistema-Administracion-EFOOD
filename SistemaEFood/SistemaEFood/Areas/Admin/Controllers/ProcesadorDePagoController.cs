using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Modelos;
using SistemaEFood.Utilidades;

namespace SistemaEFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin )]
    public class ProcesadorDePagoController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public ProcesadorDePagoController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }
        public async Task<IActionResult> Index(int? id)
        {
            ProcesadorDePago procesadorDePago = await _unidadTrabajo.ProcesadorDePago.Obtener(id.GetValueOrDefault());
            return View(procesadorDePago);
        }


        public async Task<IActionResult> Upsert(int? id)
        {
            ProcesadorDePago procesadorDePago = new ProcesadorDePago();

            if(id == null)
            {
                return View(procesadorDePago);
            }
            procesadorDePago = await _unidadTrabajo.ProcesadorDePago.Obtener(id.GetValueOrDefault());

            if(procesadorDePago == null)
            {
                return NotFound();
            }
            return View(procesadorDePago);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProcesadorDePago procesadorDePago)
        {
            if (ModelState.IsValid)
            {
                if (procesadorDePago.Id == 0)
                {
                    await _unidadTrabajo.ProcesadorDePago.Agregar(procesadorDePago);
                    TempData[DS.Exitosa] = "Procesador de pago creada exitosamente";
                }
                else
                {
                    _unidadTrabajo.ProcesadorDePago.Actualizar(procesadorDePago);
                    TempData[DS.Exitosa] = "Procesador de pago actualizada exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al grabar procesador de pago";
            return View(procesadorDePago);
        }




        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.ProcesadorDePago.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var procesadorDePagoDb = await _unidadTrabajo.ProcesadorDePago.Obtener(id);
            if (procesadorDePagoDb == null)
            {
                return Json(new { success = false, message = "Error al borrar procesador de pago Db" });
            }

            _unidadTrabajo.ProcesadorDePago.Remover(procesadorDePagoDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Procesador De Pago borrada exitosamente" });
        }
        [ActionName("ValidarEstado")]
        public async Task<IActionResult> ValidarEstado(string tipo, bool estado, int id = 0)
        {
            bool valor = false;

         

            if (tipo == null || estado == false) { return Json(new { data = false }); }
            if (tipo.ToLower() == "tarjeta de crédito o débito")
            {
                var lista = await _unidadTrabajo.ProcesadorDePago.ObtenerProcesadorTarjetas();
                if (id == 0)
                {
                    valor = lista.Any(b => b.Estado == true);
                }
                else
                {
                    valor = lista.Any(b => b.Estado == true);
                }
                if (valor)
                {
                    return Json(new { data = true });
                }
            }
            if (tipo.ToLower() == "cheque electrónico")
            {
                var lista = await _unidadTrabajo.ProcesadorDePago.ObtenerProcesadorCheques();
                if (id == 0)
                {
                    valor = lista.Any(b => b.Estado == true);
                }
                else
                {
                    valor = lista.Any(b => b.Estado == true);
                }
                if (valor)
                {
                    return Json(new { data = true });
                }
            }
            return Json(new { data = false });
        }
        [ActionName("ValidarTipo")]
        public async Task<IActionResult> ValidarTipo(string tipo, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.ProcesadorDePago.ObtenerTodos();
            if (tipo == null) { return Json(new { data = false }); }
            if (tipo.ToLower().Trim() == "efectivo") { 
            if (id == 0)
            {
                valor = lista.Any(b => b.Tipo.ToLower().Trim() == "efectivo");
            }
            else
            {
                valor = lista.Any(b => b.Tipo.ToLower().Trim() == "efectivo" && b.Id != id);
            }
            if (valor)
            {
                return Json(new { data = true });
            }
            }
            return Json(new { data = false });
        }
        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string procesador, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.ProcesadorDePago.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(b => b.Procesador.ToLower().Trim() == procesador.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(b => b.Procesador.ToLower().Trim() == procesador.ToLower().Trim() && b.Id != id);
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
