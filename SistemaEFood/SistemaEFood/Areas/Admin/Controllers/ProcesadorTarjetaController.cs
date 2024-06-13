using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Modelos;
using SistemaEFood.Modelos.ViewModels;
using SistemaEFood.Utilidades;

namespace SistemaEFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin)]
    public class ProcesadorTarjetaController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public ProcesadorTarjetaController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment webHostEnviroment)
        {
            _unidadTrabajo = unidadTrabajo;
            _webHostEnviroment = webHostEnviroment;
        }
        public IActionResult Index(int? id)
        {
            if (id != null)
            {
                ViewData["Id"] = id;
            }

            return View();
        }


        public async Task<IActionResult> Upsert(int? id)
        {
            ProcesadorTarjetaVM procesadorTarjetaVM = new ProcesadorTarjetaVM()
            {
                ProcesadorTarjeta = new ProcesadorTarjeta(),
                TarjetaLista = _unidadTrabajo.ProcesadorTarjeta.ObtenerTodosDropdownLista("Tarjeta",id),
               
            };

            if(id == null)
            {
                return View(procesadorTarjetaVM);
            }
            else
            {
                procesadorTarjetaVM.ProcesadorTarjeta.ProcesadorDePago = await _unidadTrabajo.ProcesadorDePago.Obtener(id.GetValueOrDefault());
                if(procesadorTarjetaVM.ProcesadorTarjeta.ProcesadorDePago == null)
                {
                    return NotFound();
                }
                return View(procesadorTarjetaVM);
            }

            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProcesadorTarjetaVM procesadorTarjetaVM,int id)
        {
            var usuarioNombre = User.Identity.Name;

            if (ModelState.IsValid)
            {
                if (procesadorTarjetaVM.ProcesadorTarjeta.Id == 0)
                {
                    procesadorTarjetaVM.ProcesadorTarjeta.ProcesadorId = id;
                    await _unidadTrabajo.ProcesadorTarjeta.Agregar(procesadorTarjetaVM.ProcesadorTarjeta);
                    var mensaje = TempData[DS.Exitosa] = "Tarjeta " + procesadorTarjetaVM.ProcesadorTarjeta.Tarjeta.Nombre + " asignada exitosamente";
                    await _unidadTrabajo.Bitacora.RegistrarAccion(usuarioNombre, mensaje.ToString());
                }
                else
                {
                    procesadorTarjetaVM.ProcesadorTarjeta.ProcesadorId = id;
                    await _unidadTrabajo.ProcesadorTarjeta.Agregar(procesadorTarjetaVM.ProcesadorTarjeta);
                    _unidadTrabajo.ProcesadorTarjeta.Actualizar(procesadorTarjetaVM.ProcesadorTarjeta);
                    TempData[DS.Exitosa] = "Tarjeta asignada actualizada exitosamente";
                }
                await _unidadTrabajo.Guardar();
                string returnUrl = Url.Action("Index", "ProcesadorTarjeta", new { id = procesadorTarjetaVM.ProcesadorTarjeta.ProcesadorId });

                return Redirect(returnUrl);
            }
            TempData[DS.Error] = "Error al asignar tarjeta";
            return View(procesadorTarjetaVM);
        }



        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            int id = 0; 
            if (HttpContext.Request.Query.ContainsKey("id")) 
            {
                int.TryParse(HttpContext.Request.Query["id"], out id); 
            }

            var todos = await _unidadTrabajo.ProcesadorTarjeta.ObtenerTodos(incluirPropiedades: "Tarjeta");

            var filtrados = todos.Where(t => t.ProcesadorId == id);

            return Json(new { data = filtrados });
        }



        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioNombre = User.Identity.Name;
            var procesadorTarjetaDb = await _unidadTrabajo.ProcesadorTarjeta.Obtener(id);
            if (procesadorTarjetaDb == null)
            {
                await _unidadTrabajo.BitacoraError.RegistrarError("Error al borrar Procesador de Tarjeta " + procesadorTarjetaDb.ProcesadorDePago, 400);
                return Json(new { success = false, message = "Error al borrar ProcesadorTarjeta" });
            }
            _unidadTrabajo.ProcesadorTarjeta.Remover(procesadorTarjetaDb);
            await _unidadTrabajo.Guardar();
            var mensaje = TempData[DS.Exitosa] = "Tarjeta " + procesadorTarjetaDb.ProcesadorDePago + " asignada exitosamente";
            await _unidadTrabajo.Bitacora.RegistrarAccion(usuarioNombre, mensaje.ToString());
            return Json(new { success = true, message = "ProcesadorTarjeta borrado exitosamente" });
        }
        
        #endregion

    }
}
