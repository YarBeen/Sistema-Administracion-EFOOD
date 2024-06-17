using System.Collections.Generic;
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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;

namespace PruebasEFood.Tests.Controllers
{
    public class ProcesadorTarjetaControllerTests
    {
        private readonly Mock<IUnidadTrabajo> _mockUnidadTrabajo;
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;
        private readonly ProcesadorTarjetaController procesadorTarjetaControllerPrueba;

        public ProcesadorTarjetaControllerTests()
        {
            _mockUnidadTrabajo = new Mock<IUnidadTrabajo>();
            _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            procesadorTarjetaControllerPrueba = new ProcesadorTarjetaController(_mockUnidadTrabajo.Object, _mockWebHostEnvironment.Object);

            // Simula el login y creación de un usuario
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser")
            }, "mock"));

            // Simula el HTTP necesario para el funcionamiento de la prueba
            procesadorTarjetaControllerPrueba.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            //URL Helper para el metodo Upsert
            var urlHelper = new Mock<IUrlHelper>();
            urlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>())).Returns("ProcesadorTarjeta/Index");
            procesadorTarjetaControllerPrueba.Url = urlHelper.Object;

            // Simula la existencia del TempData en forma de un Diccionario
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>())
            {
                [DS.Exitosa] = "Success message",
                [DS.Error] = "Error message"
            };
            procesadorTarjetaControllerPrueba.TempData = tempData;

            _mockUnidadTrabajo.Setup(u => u.Bitacora.RegistrarAccion(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            _mockUnidadTrabajo.Setup(u => u.BitacoraError.RegistrarError(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Act
            var result = procesadorTarjetaControllerPrueba.Index(null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_IdNoNulo_RetornaVistaYEstableceViewDataId()
        {
            // Act
            var result = procesadorTarjetaControllerPrueba.Index(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(1, viewResult.ViewData["Id"]);
        }

        [Fact]
        public async Task Upsert_Get_IdEsNulo_RetornaVistaConNuevoProcesadorTarjetaVM()
        {
            // Arrange
            _mockUnidadTrabajo.Setup(u => u.ProcesadorTarjeta.ObtenerTodosDropdownLista(It.IsAny<string>(), null))
                .Returns(new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>());

            // Act
            var result = await procesadorTarjetaControllerPrueba.Upsert(null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ProcesadorTarjetaVM>(viewResult.Model);
            Assert.NotNull(model.ProcesadorTarjeta);
            Assert.Empty(model.TarjetaLista);
        }

        [Fact]
        public async Task Upsert_Get_IdNoEsNulo_RetornaVistaConProcesadorTarjetaVM()
        {
            // Arrange
            var procesadorDePago = new ProcesadorDePago { Id = 1, NombreOpcionDePago = "Pago Test" };
            _mockUnidadTrabajo.Setup(u => u.ProcesadorTarjeta.ObtenerTodosDropdownLista(It.IsAny<string>(), 1))
                .Returns(new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>());
            _mockUnidadTrabajo.Setup(u => u.ProcesadorDePago.Obtener(1))
                .ReturnsAsync(procesadorDePago);

            // Act
            var result = await procesadorTarjetaControllerPrueba.Upsert(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ProcesadorTarjetaVM>(viewResult.Model);
            Assert.Equal(procesadorDePago.Id, model.ProcesadorTarjeta.ProcesadorDePago.Id);
        }

        [Fact]
        public async Task Upsert_Get_IdNoEsNulo_RetornaNotFoundCuandoProcesadorDePagoEsNulo()
        {
            // Arrange
            _mockUnidadTrabajo.Setup(u => u.ProcesadorDePago.Obtener(It.IsAny<int>())).ReturnsAsync((ProcesadorDePago)null);
            _mockUnidadTrabajo.Setup(u => u.ProcesadorTarjeta.ObtenerTodosDropdownLista(It.IsAny<string>(), It.IsAny<int?>()))
                .Returns(new List<SelectListItem>());

            // Act
            var result = await procesadorTarjetaControllerPrueba.Upsert(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task Upsert_Post_ModeloValido_IdNoEsCero_ActualizaProcesadorTarjeta()
        {
            // Arrange
            var procesadorTarjetaVM = new ProcesadorTarjetaVM
            {
                ProcesadorTarjeta = new ProcesadorTarjeta { Id = 1, ProcesadorId = 1, TarjetaId = 1 }
            };

            _mockUnidadTrabajo.Setup(u => u.ProcesadorTarjeta.Actualizar(It.IsAny<ProcesadorTarjeta>()));
            _mockUnidadTrabajo.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _mockUnidadTrabajo.Setup(u => u.Bitacora.RegistrarAccion(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            var result = await procesadorTarjetaControllerPrueba.Upsert(procesadorTarjetaVM, 1);

            // Assert
            _mockUnidadTrabajo.Verify(u => u.ProcesadorTarjeta.Actualizar(It.Is<ProcesadorTarjeta>(p => p == procesadorTarjetaVM.ProcesadorTarjeta)), Times.Once);
            _mockUnidadTrabajo.Verify(u => u.Guardar(), Times.Once);

            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Contains("ProcesadorTarjeta/Index", redirectResult.Url);
        }

        [Fact]
        public async Task Upsert_Post_ModeloNoValido_RetornaVistaConModelo()
        {
            // Arrange
            var procesadorTarjetaVM = new ProcesadorTarjetaVM
            {
                ProcesadorTarjeta = new ProcesadorTarjeta { Id = 1, ProcesadorId = 1, TarjetaId = 1 }
            };

            procesadorTarjetaControllerPrueba.ModelState.AddModelError("Error", "Modelo no válido");

            // Act
            var result = await procesadorTarjetaControllerPrueba.Upsert(procesadorTarjetaVM, 1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ProcesadorTarjetaVM>(viewResult.Model);
            Assert.Equal(procesadorTarjetaVM, model);
        }
    }
}
