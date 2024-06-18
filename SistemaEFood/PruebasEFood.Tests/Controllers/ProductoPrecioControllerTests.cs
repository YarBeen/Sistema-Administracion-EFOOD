using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;
using SistemaEFood.Areas.Admin.Controllers;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Modelos;
using SistemaEFood.Modelos.ViewModels;
using SistemaEFood.Utilidades;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using SistemaEFood.AccesoDatos.Migrations;
using Microsoft.AspNetCore.Mvc.Routing;

namespace PruebasEFood.Tests.Controllers
{
    public class ProductoPrecioControllerTests
    {
        private readonly Mock<IUnidadTrabajo> _mockUnidadTrabajo;
        private readonly Mock<IUrlHelper> _mockUrlHelper;
        private readonly ProductoPrecioController productoPrecioControllerPrueba;

        public ProductoPrecioControllerTests()
        {
            _mockUnidadTrabajo = new Mock<IUnidadTrabajo>();
            _mockUrlHelper = new Mock<IUrlHelper>();
            productoPrecioControllerPrueba = new ProductoPrecioController(_mockUnidadTrabajo.Object);

            // Simula el login y creación de un usuario
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser")
            }, "mock"));

            // Configurar el comportamiento esperado de UrlHelper
            _mockUrlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("mocked-url");

            // Simula el HTTP para el funcionamiento de la prueba
            productoPrecioControllerPrueba.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            //Simulacion del TempData en forma de un Diccionario
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>())
            {
                [DS.Exitosa] = "Success message",
                [DS.Error] = "Error message"
            };
            productoPrecioControllerPrueba.TempData = tempData;

            //Simulacion Bitacora
            _mockUnidadTrabajo.Setup(u => u.Bitacora.RegistrarAccion(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            _mockUnidadTrabajo.Setup(u => u.BitacoraError.RegistrarError(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Act
            var result = productoPrecioControllerPrueba.Index(null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_IdNoNulo_RetornaVistaYEstableceViewDataId()
        {
            // Act
            var result = productoPrecioControllerPrueba.Index(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(1, viewResult.ViewData["Id"]);
        }

        [Fact]
        public void Consultar_ReturnsViewResult()
        {
            // Act
            var result = productoPrecioControllerPrueba.Consultar(null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Consultar_IdNoNulo_RetornaVistaYEstableceViewDataId()
        {
            // Act
            var result = productoPrecioControllerPrueba.Consultar(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(1, viewResult.ViewData["Id"]);
        }

        [Fact]
        public async Task Upsert_Get_IdEsNulo_RetornaVistaConNuevoProductoPrecioVM()
        {
            // Arrange
            _mockUnidadTrabajo.Setup(u => u.ProductoPrecio.ObtenerTipoPrecios(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>());

            // Act
            var result = await productoPrecioControllerPrueba.Upsert(1, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ProductoPrecioVM>(viewResult.Model);
            Assert.NotNull(model);
            Assert.Equal(1, model.idProducto);
        }

        [Fact]
        public async Task Upsert_Get_IdNoEsNulo_RetornaVistaConProductoPrecioVM()
        {
            // Arrange
            var productoID = 1;
            var relacionId = 1;
            var productoPrecio = new SistemaEFood.Modelos.ProductoPrecio { Id = 1, Idprecio = 2 };
            var listaPrecios = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Precio 1" },
                new SelectListItem { Value = "2", Text = "Precio 2" }
            };

            _mockUnidadTrabajo.Setup(u => u.ProductoPrecio.ObtenerTipoPrecios(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(listaPrecios);

            _mockUnidadTrabajo.Setup(u => u.ProductoPrecio.ObtenerPrimero(
                It.IsAny<Expression<Func<SistemaEFood.Modelos.ProductoPrecio, bool>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()
                ))
                .ReturnsAsync(productoPrecio);

            // Act
            var result = await productoPrecioControllerPrueba.Upsert(1, 1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ProductoPrecioVM>(viewResult.Model);
            Assert.Equal(productoPrecio.Id, model.idRelacion);
            Assert.Equal(productoPrecio.Idprecio, model.tipoPrecioID);
        }

        [Fact]
        public async Task Upsert_Get_IdNoEsNulo_RetornaNotFoundCuandoProductoPrecioEsNulo()
        {

            // Arrange
            var productoPrecio = new SistemaEFood.Modelos.ProductoPrecio { Id = 1, Idprecio = 2 };
            var TipoPrecio = new SistemaEFood.Modelos.TipoPrecio { Id = 1, Nombre = "TipoPrecio" };
            _mockUnidadTrabajo.Setup(u => u.ProductoPrecio.ObtenerPrimero(
                It.IsAny<Expression<Func<SistemaEFood.Modelos.ProductoPrecio, bool>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()
                ));

            // Act
            var result = await productoPrecioControllerPrueba.Upsert(1, 1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        
        [Fact]
        public async Task Upsert_Post_ModeloNoValido_RetornaVistaConModelo()
        {
            // Arrange
            var productoPrecioVM = new ProductoPrecioVM
            {
                idProducto = 1,
                tipoPrecioID = 1,
                monto = 100,
                idRelacion = 1
            };
            productoPrecioControllerPrueba.ModelState.AddModelError("Error", "Modelo no válido");

            // Act
            var result = await productoPrecioControllerPrueba.Upsert(productoPrecioVM);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ProductoPrecioVM>(viewResult.Model);
            Assert.Equal(productoPrecioVM, model);
        }
    }
}
