using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaEFood.AccesoDatos.Data;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.AccesoDatos.Repositorio
{
    public class UsuarioRepositorio : Repositorio<Usuario>, IUsuarioRepositorio
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public UsuarioRepositorio(ApplicationDbContext db, UserManager<IdentityUser> userManager) : base(db)
        {
            _db = db;
            _userManager = userManager;
        }
        public void Actualizar(Usuario usuario)
        {
            var usuarioDB = _db.Usuarios.FirstOrDefault(b => b.Id == usuario.Id);

            if (usuarioDB != null)
            {
                usuarioDB.Email = usuario.Email;
                usuarioDB.UserName = usuario.UserName;
                usuarioDB.PreguntaSeguridad= usuario.PreguntaSeguridad;
                usuarioDB.RespuestaSeguridad = usuario.RespuestaSeguridad;
                usuarioDB.Role = usuario.Role; 
                //Lo de password creo xd
                _db.SaveChanges();
                _db.Database.ExecuteSqlRaw("UPDATE AspNetUserRoles SET RoleId = {0} WHERE UserId = {1}", usuario.Role, usuarioDB.Id);
            }
        }

        public async Task<bool> ActualizarPasswordAsync(string userId, string newPassword)
        {
            
            
           
            var user = await _userManager.FindByIdAsync(userId);
           

           
            if (user == null)
            {
                
                return false; // User not found
            }
            Console.WriteLine(user.Email);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            IdentityResult result = await _userManager.ResetPasswordAsync(user, token, newPassword);
           
            Console.WriteLine(result.ToString());
            Console.WriteLine(result.Succeeded);
           
            
            Console.WriteLine(result.Errors);

            
            return result.Succeeded;
        }

        public IEnumerable<SelectListItem> ObtenerRoles()
        {
            return _db.Roles.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id
            });
        }

        public async Task<Usuario> ObtenerUsuarioPorID(string id)
        {
            var user= await _db.FindAsync<Usuario>(id);
            if (user != null)
            {
                var UserRoleRelation =  await _db.UserRoles.FirstOrDefaultAsync(r => r.UserId == user.Id);
                if (UserRoleRelation != null)
                {
                    var roleFromUser = await _db.Roles.FirstOrDefaultAsync(r => r.Id == UserRoleRelation.RoleId);
                    if (roleFromUser != null) {
                        user.Role = roleFromUser.Name;
                    }
                }
                
            }
            
           

            return user;
            /*var usuarioDB = _db.Usuarios.FirstOrDefault(b => b.Id == id);
            if (usuarioDB == null)
            {
                return NotFound(); // Devuelve un error 404 Not Found
            }
            return Ok(usuarioDB);*/
        }
    }
}
