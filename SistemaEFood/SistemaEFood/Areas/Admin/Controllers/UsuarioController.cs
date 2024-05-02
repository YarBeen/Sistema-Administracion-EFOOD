using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEFood.AccesoDatos.Data;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Utilidades;

namespace SistemaEFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin +"," +DS.Role_Seguridad)]
    public class UsuarioController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly ApplicationDbContext _db;

        public UsuarioController(IUnidadTrabajo unidadTrabajo, ApplicationDbContext db)
        {
                _unidadTrabajo = unidadTrabajo;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var usuarioLista = await _unidadTrabajo.Usuario.ObtenerTodos();
            var userRole = await _db.UserRoles.ToListAsync();
            var roles = await _db.Roles.ToListAsync();
            foreach (var usuario in usuarioLista)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == usuario.Id).RoleId;
                usuario.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
            }
            return Json(new { data = usuarioLista });
        }
        [HttpPost]
        public async Task<IActionResult> BloquearDesbloquear([FromBody] string id)
        {
            var usuario = await _unidadTrabajo.Usuario.ObtenerPrimero(u => u.Id == id);
            if (usuario == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Error de usuario"
                });
            }
                if (usuario.LockoutEnd != null && usuario.LockoutEnd > DateTime.Now)
                {

                    //Si la fecha es mayor a hoy esta bloqueado
                    usuario.LockoutEnd = DateTime.Now;

                }
                else {
                    usuario.LockoutEnd = DateTime.Now.AddYears(1250);
                }
            await _unidadTrabajo.Guardar();
            return Json(new
            {
                success = true,
                message = " La operacion fue un exito"
            });
        }
        #endregion

    }
}
