using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Modelos;
using SistemaEFood.Modelos.ViewModels;
using SistemaEFood.Utilidades;

namespace SistemaEFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Mantenimiento)]
    public class ProductoController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public ProductoController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment webHostEnviroment)
        {
            _unidadTrabajo = unidadTrabajo;
            _webHostEnviroment = webHostEnviroment;
        }
        public IActionResult Index()
        {
            return View();
        }

        
        public async Task<IActionResult> Consultar()
        {
            ProductoVM productoVM = new ProductoVM()
            {
                Producto = new Producto(),
                LineaComidaLista = _unidadTrabajo.Producto.ObtenerTodosDropdownLista("LineaComida"),
                ProductosLista = await _unidadTrabajo.Producto.ObtenerTodos()
            };
            return View(productoVM);
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            ProductoVM productoVM = new ProductoVM()
            {
                Producto = new Producto(),
                LineaComidaLista = _unidadTrabajo.Producto.ObtenerTodosDropdownLista("LineaComida")
            };

            if(id == null)
            {
                return View(productoVM);
            }
            else
            {
                productoVM.Producto = await _unidadTrabajo.Producto.Obtener(id.GetValueOrDefault());
                if(productoVM.Producto == null)
                {
                    return NotFound();
                }
                return View(productoVM);
            }

            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductoVM productoVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnviroment.WebRootPath;

                if(productoVM.Producto.Id == 0)
                {
                    string upload = webRootPath + DS.ImagenRuta;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productoVM.Producto.ImagenUrl = fileName + extension;
                    await _unidadTrabajo.Producto.Agregar(productoVM.Producto);
                }
                else
                {
                    var objProducto = await _unidadTrabajo.Producto.ObtenerPrimero(p => p.Id == productoVM.Producto.Id, isTracking: false);
                    if (files.Count > 0) // Si se carga una nueva Imagen para el producto existente
                    {
                        string upload = webRootPath + DS.ImagenRuta;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var anteriorFile = Path.Combine(upload, objProducto.ImagenUrl);
                        if (System.IO.File.Exists(anteriorFile))
                        {
                            System.IO.File.Delete(anteriorFile);
                        }
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productoVM.Producto.ImagenUrl = fileName + extension;
                    }
                    else
                    {
                        productoVM.Producto.ImagenUrl = objProducto.ImagenUrl;
                    }

                    _unidadTrabajo.Producto.Actualizar(productoVM.Producto);

                }
                TempData[DS.Exitosa] = "Transacción Exitosa!";
                await _unidadTrabajo.Guardar();
                return View("Index");
            }
            productoVM.LineaComidaLista = _unidadTrabajo.Producto.ObtenerTodosDropdownLista("LineaComida");
            return View(productoVM);
        }



        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Producto.ObtenerTodos(incluirPropiedades:"LineaComida");
            return Json(new { data = todos });
        }

        
        [HttpGet]
        public async Task<IActionResult> ConsultarConFiltro(int? idLineaComida)
        {
            var productoVM = new ProductoVM();

            productoVM.LineaComidaLista = _unidadTrabajo.Producto.ObtenerTodosDropdownLista("LineaComida");

            if (idLineaComida.HasValue)
            {
                productoVM.ProductosLista = await _unidadTrabajo.Producto.ObtenerTodosPorLineaComida(idLineaComida.Value);
            }
            else
            {
                // Si no se proporina un ID de Linea de Comida, se obtienen todos los productos
                productoVM.ProductosLista = await _unidadTrabajo.Producto.ObtenerTodos();
            }
            var resultados = productoVM.ProductosLista;
            return Json(new { data = resultados }); 
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var productoDb = await _unidadTrabajo.Producto.Obtener(id);
            if (productoDb == null)
            {
                var mensajeError = TempData[DS.Error] = "Error al borrar producto";
                await _unidadTrabajo.BitacoraError.RegistrarError(mensajeError.ToString(), 400);
                return Json(new { success = false, message = "Error al borrar producto" });
            }

            string upload = _webHostEnviroment.WebRootPath + DS.ImagenRuta;
            var anteriorFile = Path.Combine(upload, productoDb.ImagenUrl);
            if (System.IO.File.Exists(anteriorFile))
            {
                System.IO.File.Delete(anteriorFile);
            }

            _unidadTrabajo.Producto.Remover(productoDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Producto borrada exitosamente" });
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Producto.ObtenerTodos();
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
