using Microsoft.AspNetCore.Mvc;
using SistemaEFood.AccesoDatos.Data;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Modelos;
using System;
using System.Collections.Generic;
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

        public IActionResult ObtenerUsuarioPorID(string id)
        {
            var usuarioDB = _db.Usuarios.FirstOrDefault(b => b.Id == id);
            if (usuarioDB == null)
            {
                return NotFound(); // Devuelve un error 404 Not Found
            }
            return Ok(usuarioDB);
        }
    }
}
