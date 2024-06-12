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

        public async Task<IActionResult> Upsert(int productoID, int? relacionId)
        {
           

            ProductoPrecioVM productoPrecioVM = new ProductoPrecioVM()
            {
                idProducto= productoID,
                
                ListaPrecios = _unidadTrabajo.ProductoPrecio.ObtenerTipoPrecios("TipoPrecio", productoID)
            }; 
           

            if (relacionId == null)
            {
                return View(productoPrecioVM);
            }
            else
            {
            
                productoPrecioVM.idRelacion = relacionId.Value;
                var productoPrecioOBJ = await _unidadTrabajo.ProductoPrecio.ObtenerPrimero(X => X.Id == productoPrecioVM.idRelacion);
                if (productoPrecioOBJ == null)
                {
                    return NotFound();  
                }


              //  productoPrecioVM.tipoPrecioNombre = productoPrecioVM.ListaPrecios.Where(x => x.Value.Equals(productoPrecioOBJ.Idprecio.ToString())).Select(x=>x.Text).FirstOrDefault();
                productoPrecioVM.tipoPrecioID = productoPrecioOBJ.Idprecio;


                return View(productoPrecioVM);
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductoPrecioVM productoPrecioVM)
        {
            
           
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values
                               .SelectMany(v => v.Errors)
                               .Select(e => e.ErrorMessage)
                               .ToList();

                foreach (var error in allErrors)
                {
                    Console.WriteLine(error);
                }
            }
            if (ModelState.IsValid)
            {
                if (productoPrecioVM.idRelacion == 0)
                {
                    //productoPrecioVM.productoPrecio.Idproducto = id;
                    ProductoPrecio existePrecio = await _unidadTrabajo.ProductoPrecio.ObtenerPrimero(X => X.Idproducto == productoPrecioVM.idProducto && X.Idprecio == productoPrecioVM.tipoPrecioID);
                    if(existePrecio != null) 
                    {
                        TempData[DS.Error] = "precio ya existente";
                        await _unidadTrabajo.BitacoraError.RegistrarError("Se intento ingresar un precio ya existente", 400);
                    }
                    else 
                    {
                        var productoPrecio = new ProductoPrecio();
                        productoPrecio.Idproducto = productoPrecioVM.idProducto;   
                        productoPrecio.Idprecio=productoPrecioVM.tipoPrecioID;
                        productoPrecio.Monto = productoPrecioVM.monto;

                        productoPrecio.TipoPrecio = await _unidadTrabajo.TipoPrecio.ObtenerPrimero(X => X.Id == productoPrecioVM.tipoPrecioID);
                        productoPrecio.Producto = await _unidadTrabajo.Producto.ObtenerPrimero(X => X.Id == productoPrecioVM.idProducto);
                        
                        await _unidadTrabajo.ProductoPrecio.Agregar(productoPrecio);

                        TempData[DS.Exitosa] = "Precio creado exitosamente";
                    }
                    
                }
                else
                {
                    var productoPrecio = new ProductoPrecio();
                    productoPrecio.Idproducto = productoPrecioVM.idProducto;
                    productoPrecio.Idprecio = productoPrecioVM.tipoPrecioID;
                    productoPrecio.Monto = productoPrecioVM.monto;
                    productoPrecio.Id = productoPrecioVM.idRelacion;

                    productoPrecio.TipoPrecio = await _unidadTrabajo.TipoPrecio.ObtenerPrimero(X => X.Id == productoPrecioVM.tipoPrecioID);
                    productoPrecio.Producto = await _unidadTrabajo.Producto.ObtenerPrimero(X => X.Id == productoPrecioVM.idProducto);
                    
                    _unidadTrabajo.ProductoPrecio.Actualizar(productoPrecio);
                    TempData[DS.Exitosa] = "Precio actualizado exitosamente";
                }
                await _unidadTrabajo.Guardar();
                string returnUrl = Url.Action("Index", "ProductoPrecio", new { id = productoPrecioVM.idProducto });
                return Redirect(returnUrl);
            }
            var mensajeError= "Error al grabar precio";
            TempData[DS.Error] = mensajeError;
            await _unidadTrabajo.BitacoraError.RegistrarError(mensajeError.ToString(), 300);
            //productoPrecioVM.ListaPrecios = _unidadTrabajo.ProductoPrecio.ObtenerTipoPrecios("TipoPrecio", id);
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
