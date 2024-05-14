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

        public UsuarioRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(Usuario usuario)
        {
            var usuarioDB = _db.Usuarios.FirstOrDefault(b => b.Id == usuario.Id);

            if (usuarioDB != null)
            {
                usuarioDB.Email = usuario.Email;
                usuarioDB.UserName = usuario.UserName;
                usuarioDB.Role = usuario.Role; 
                //Lo de password creo xd
                _db.SaveChanges();
            }
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
