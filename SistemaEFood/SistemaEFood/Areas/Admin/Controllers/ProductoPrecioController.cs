using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEFood.AccesoDatos.Repositorio;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Modelos;
using SistemaEFood.Modelos.ViewModels;
using SistemaEFood.Utilidades;

namespace SistemaEFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Mantenimiento)]
    public class ProductoPrecioController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        
        public ProductoPrecioController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }
        public IActionResult Index(int? id)
        {
            if (id != null)
            {
                ViewData["Id"] = id;
            }

            return View();
        }

        public IActionResult Consultar(int? id)
        {
            if(id != null)
            {
                ViewData["Id"] = id;
            }
            return View();
        }

        public async Task<IActionResult> Upsert(int? id, int? relacionId)
        {
            ProductoPrecioVM productoPrecioVM = new ProductoPrecioVM()
            {
                productoPrecio = new ProductoPrecio(),
                ListaPrecios = _unidadTrabajo.ProductoPrecio.ObtenerTipoPrecios("TipoPrecio", id)
            };
            
            if (relacionId == null)
            {
                return View(productoPrecioVM);
            }
            else
            {
                Console.WriteLine("Upsert 1-> Numeeeeero: " + id); // relacion
                productoPrecioVM.productoPrecio.Producto = await _unidadTrabajo.Producto.Obtener(id.GetValueOrDefault());
                if (productoPrecioVM.productoPrecio == null)
                {
                    return NotFound();
                }
                productoPrecioVM.productoPrecio.Idproducto = id.Value;
                productoPrecioVM.productoPrecio.Id = relacionId.Value;
                return View(productoPrecioVM);
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductoPrecioVM productoPrecioVM, int id)
        {
            Console.WriteLine("Upsert 2-> Numeeeeero: " + id);
            if (ModelState.IsValid)
            {
                if (productoPrecioVM.productoPrecio.Id == 0)
                {
                    productoPrecioVM.productoPrecio.Idproducto = id;
                    ProductoPrecio existePrecio = await _unidadTrabajo.ProductoPrecio.ObtenerPrimero(X => X.Idproducto == productoPrecioVM.productoPrecio.Idproducto && X.Idprecio == productoPrecioVM.productoPrecio.Idprecio);
                    if(existePrecio != null) 
                    {
                        TempData[DS.Error] = "Este precio ya existe";
                    }
                    else 
                    {
                        await _unidadTrabajo.ProductoPrecio.Agregar(productoPrecioVM.productoPrecio);
                        TempData[DS.Exitosa] = "Precio creado exitosamente";
                    }
                    
                }
                else
                {
                    productoPrecioVM.productoPrecio.Idproducto = id;
                    _unidadTrabajo.ProductoPrecio.Actualizar(productoPrecioVM.productoPrecio);
                    TempData[DS.Exitosa] = "Precio actualizado exitosamente";
                }
                await _unidadTrabajo.Guardar();
                string returnUrl = Url.Action("Index", "ProductoPrecio", new { id = productoPrecioVM.productoPrecio.Idproducto });
                return Redirect(returnUrl);
            }
            TempData[DS.Error] = "Error al grabar precio";
            productoPrecioVM.ListaPrecios = _unidadTrabajo.ProductoPrecio.ObtenerTipoPrecios("TipoPrecio", id);
            return View(productoPrecioVM);
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

            var todos = await _unidadTrabajo.ProductoPrecio.ObtenerTodos(incluirPropiedades: "TipoPrecio");

            var filtrados = todos.Where(t => t.Idproducto == id);
            return Json(new { data = filtrados });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var ProductoPrecioDb = await _unidadTrabajo.ProductoPrecio.Obtener(id);
            if (ProductoPrecioDb == null)
            {
                return Json(new { success = false, message = "Error al borrar Precio" });
            }

            _unidadTrabajo.ProductoPrecio.Remover(ProductoPrecioDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "precio borrado exitosamente" });
        }
        #endregion

    }
}
