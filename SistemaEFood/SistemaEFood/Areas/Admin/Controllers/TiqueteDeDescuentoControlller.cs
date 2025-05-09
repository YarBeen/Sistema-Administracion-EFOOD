﻿using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Modelos;
using SistemaEFood.Utilidades;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SistemaEFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin+"," +DS.Role_Mantenimiento)]
    public class TiqueteDeDescuentoController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public TiqueteDeDescuentoController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }
        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Upsert(int? id)
        {
            TiqueteDeDescuento tiqueteDeDescuento = new TiqueteDeDescuento();

            if(id == null)
            {
                return View(tiqueteDeDescuento);
            }
            tiqueteDeDescuento = await _unidadTrabajo.TiqueteDeDescuento.Obtener(id.GetValueOrDefault());

            if(tiqueteDeDescuento == null)
            {
                return NotFound();
            }
            return View(tiqueteDeDescuento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TiqueteDeDescuento tiqueteDeDescuento)
        {
            var usuarioNombre = User.Identity.Name;
            if (ModelState.IsValid)
            {
                if (tiqueteDeDescuento.Id == 0)
                {
                    await _unidadTrabajo.TiqueteDeDescuento.Agregar(tiqueteDeDescuento);
                    TempData[DS.Exitosa] = "Tiquete de descuento creada exitosamente";
                    await _unidadTrabajo.Bitacora.RegistrarAccion(usuarioNombre, "Se creó el tiquete de descuento " + tiqueteDeDescuento.Nombre + " de forma exitosa");
                }
                else
                {
                    _unidadTrabajo.TiqueteDeDescuento.Actualizar(tiqueteDeDescuento);
                    TempData[DS.Exitosa] = "Tiquete de descuento actualizada exitosamente";
                    await _unidadTrabajo.Bitacora.RegistrarAccion(usuarioNombre, "Se actualizó el tiquete de descuento " + tiqueteDeDescuento.Nombre + " de forma exitosa");
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al grabar Tiquete de descuento";
            await _unidadTrabajo.BitacoraError.RegistrarError("Error al grabar un tiquete de descuento", 300);
            return View(tiqueteDeDescuento);
        }




        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.TiqueteDeDescuento.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioNombre = User.Identity.Name;
            var tiqueteDeDescuentoDb = await _unidadTrabajo.TiqueteDeDescuento.Obtener(id);
            if (tiqueteDeDescuentoDb == null)
            {
                await _unidadTrabajo.BitacoraError.RegistrarError("Error al borrar el tiquete de descuento " + tiqueteDeDescuentoDb.Nombre, 300);
                return Json(new { success = false, message = "Error al borrar Tiquete de descuento Db" });
            }

            _unidadTrabajo.TiqueteDeDescuento.Remover(tiqueteDeDescuentoDb);
            await _unidadTrabajo.Guardar();
            await _unidadTrabajo.Bitacora.RegistrarAccion(usuarioNombre, "Se borró el tiquete de descuento " + tiqueteDeDescuentoDb.Nombre + " de forma exitosa");
            return Json(new { success = true, message = "Tiquete de descuento borrada exitosamente" });
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            if (nombre == null)
            {
                return Json(new { data = false });

            }
            bool valor = false;
            var lista = await _unidadTrabajo.TiqueteDeDescuento.ObtenerTodos();
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

        [ActionName("ValidarCodigo")]
        public async Task<IActionResult> ValidarCodigo(string codigo, int id = 0)
        {
            if (codigo == null)
            {
                return Json(new { data = false });

            }
            bool valor = false;
            var lista = await _unidadTrabajo.TiqueteDeDescuento.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(b => b.Codigo.ToLower().Trim() == codigo.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(b => b.Codigo.ToLower().Trim() == codigo.ToLower().Trim() && b.Id != id);
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
