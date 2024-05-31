using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEFood.AccesoDatos.Data;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Areas.Admin.Requests;
using SistemaEFood.Modelos;
using SistemaEFood.Modelos.ViewModels;
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
        public async Task<IActionResult> Upsert(string? id)
        {
            UsuarioVM userVM = new UsuarioVM()
            {
                UserModel= new Usuario(),
                ListaRoles= _unidadTrabajo.Usuario.ObtenerRoles()
            };

            if (id == null)
            {
                //Ver esto conoce pura vida
                return View(userVM);
            }
            var userForView = await _unidadTrabajo.Usuario.ObtenerUsuarioPorID(id);

            if (userForView == null)
            {
                return NotFound();
            }
            userVM.UserModel = userForView;
            return View(userVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(UsuarioVM usuarioVM)
        {
            Console.WriteLine("Addres of user is. " + usuarioVM.GetHashCode());
            Console.WriteLine(usuarioVM.UserModel.Id);
            Console.WriteLine(usuarioVM.UserModel.UserName);
            Console.WriteLine(ModelState.IsValid);
            Console.WriteLine(ModelState.Values);
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    foreach (var error in state.Errors)
                    {
                        Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
                    }
                }
            }


            if (ModelState.IsValid)
            {
                Console.WriteLine("Is valid");
                if (usuarioVM.UserModel.Id == "")
                {
                    await _unidadTrabajo.Usuario.Agregar(usuarioVM.UserModel);
                    TempData[DS.Exitosa] = "Usuario creado exitosamente";
                }
                else
                {
                    Console.WriteLine("Updating");
                    _unidadTrabajo.Usuario.Actualizar(usuarioVM.UserModel);
                    TempData[DS.Exitosa] = "Usuario";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al grabar usuario";
            
            return View(usuarioVM);
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

        [HttpPost]
        public async Task<IActionResult> CambiarContrasenna([FromBody] ChangePasswordRequest request)
        {

            var id = request.UserID;
            var password = request.Password;
            
           
            var usuario = await _unidadTrabajo.Usuario.ObtenerPrimero(u => u.Id == id);
            if (usuario == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Error de usuario"
                });
            }
            var result = await _unidadTrabajo.Usuario.ActualizarPasswordAsync(id,password);
            
            return Json(new
            {
                success = true,
                message = " La operacion fue un exito"
            });
        }

        [HttpPost]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
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

        [HttpPost]
        public async Task<IActionResult> CambiarRole([FromBody] string id)
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
            else
            {
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
