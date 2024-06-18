using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Areas.Admin.Controllers;
using SistemaEFood.Modelos.ViewModels;
using SistemaEFood.Modelos;
using SistemaEFood.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SistemaEFood.Utilidades;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.VisualStudio.Services.Users;
using System.Security.Claims;
using System.Linq.Expressions;

namespace PruebasEFood.Tests.Controllers
{
    public class ProductoControllerTests
    {
        private readonly Mock<IUnidadTrabajo> _mockUnidadTrabajo;
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;
        private readonly ProductoController _productoController;
        private readonly Mock<IStorageService> _mockStorageService;
        public ProductoControllerTests()
        {
            _mockUnidadTrabajo = new Mock<IUnidadTrabajo>();
            _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            _mockStorageService = new Mock<IStorageService>();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser")
            }, "mock"));
            _productoController = new ProductoController(_mockUnidadTrabajo.Object, _mockWebHostEnvironment.Object, _mockStorageService.Object);
            _productoController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>())
            {
                [DS.Exitosa] = "Success message",
                [DS.Error] = "Error message"
            };
            _productoController.TempData = tempData;
            _mockUnidadTrabajo.Setup(u => u.Bitacora.RegistrarAccion(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            _mockUnidadTrabajo.Setup(u => u.BitacoraError.RegistrarError(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);
        }
        [Fact]
        public async Task Upsert_IdValido_DebeRetornarVistaConProductoExistente()
        {
            // Arrange
            var idProductoExistente = 1;
            var productoExistente = new Producto { Id = idProductoExistente, Nombre = "ProductoExistente" };
            _mockUnidadTrabajo.Setup(u => u.Producto.ObtenerTodosDropdownLista("LineaComida"))
                              .Returns(new List<SelectListItem>());
            _mockUnidadTrabajo.Setup(u => u.Producto.Obtener(idProductoExistente))
                              .ReturnsAsync(productoExistente);

            // Act
            var result = await _productoController.Upsert(idProductoExistente);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ProductoVM>(viewResult.Model);
            Assert.Equal(productoExistente, model.Producto);

            _mockUnidadTrabajo.Verify(u => u.Producto.ObtenerTodosDropdownLista("LineaComida"), Times.Once);
            _mockUnidadTrabajo.Verify(u => u.Producto.Obtener(idProductoExistente), Times.Once);
        }
        [Fact]
        public async Task Upsert_Post_ModeloValido_CreaNuevoProducto_ConImagen()
        {
            // Arrange
            var productoVM = new ProductoVM
            {
                Producto = new Producto { Id = 0, Nombre = "Nuevo Producto", Contenido = "Contenido del producto", LineaComidaId = 1 },
                LineaComidaLista = new List<SelectListItem>()
            };

            var formFiles = new FormFileCollection();
            formFiles.Add(new FormFile(Stream.Null, 0, 0, "file", "imagen.png"));

            _mockUnidadTrabajo.Setup(u => u.Producto.Agregar(It.IsAny<Producto>()))
                              .Callback<Producto>(p => p.Id = 1)
                              .Returns(Task.CompletedTask);
            _mockUnidadTrabajo.Setup(u => u.Guardar()).Returns(Task.CompletedTask);

            _mockWebHostEnvironment.Setup(w => w.WebRootPath).Returns("wwwroot");
            _mockWebHostEnvironment.Setup(w => w.EnvironmentName).Returns("Development");

            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controllerContext.HttpContext.Request.Form = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(), formFiles);

            _productoController.ControllerContext = controllerContext;

            // Act
            var result = await _productoController.Upsert(productoVM);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Index", viewResult.ViewName);
          

            _mockUnidadTrabajo.Verify(u => u.Producto.Agregar(It.IsAny<Producto>()), Times.Once);
            _mockUnidadTrabajo.Verify(u => u.Guardar(), Times.Once);
        }
        [Fact]
        public async Task Upsert_Post_ModeloValido_ActualizaProducto_ConImagen()
        {
            // Arrange
            var productoExistente = new Producto { Id = 1, Nombre = "Producto Existente", Contenido = "Contenido anterior", LineaComidaId = 2, ImagenUrl = "imagen_anterior.png" };
            var productoVM = new ProductoVM
            {
                Producto = productoExistente,
                LineaComidaLista = new List<SelectListItem>()
            };

            var formFiles = new FormFileCollection();
            formFiles.Add(new FormFile(Stream.Null, 0, 0, "file", "nueva_imagen.png"));

            _mockUnidadTrabajo.Setup(u => u.Producto.ObtenerPrimero(It.IsAny<Expression<Func<Producto, bool>>>(), null, false))
                              .ReturnsAsync(productoExistente);
            _mockUnidadTrabajo.Setup(u => u.Producto.Actualizar(It.IsAny<Producto>()));
            _mockUnidadTrabajo.Setup(u => u.Guardar()).Returns(Task.CompletedTask);

            _mockWebHostEnvironment.Setup(w => w.WebRootPath).Returns("wwwroot");
            _mockWebHostEnvironment.Setup(w => w.EnvironmentName).Returns("Development");

            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controllerContext.HttpContext.Request.Form = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(), formFiles);

            _productoController.ControllerContext = controllerContext;

            // Act
            var result = await _productoController.Upsert(productoVM);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Index", viewResult.ViewName);


            _mockUnidadTrabajo.Verify(u => u.Producto.ObtenerPrimero(It.IsAny<Expression<Func<Producto, bool>>>(), null,  false), Times.Once);
            _mockUnidadTrabajo.Verify(u => u.Producto.Actualizar(It.IsAny<Producto>()), Times.Once);
            _mockUnidadTrabajo.Verify(u => u.Guardar(), Times.Once);
        }
       
    }
}
