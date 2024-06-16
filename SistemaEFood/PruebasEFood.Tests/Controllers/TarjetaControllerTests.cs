using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Areas.Admin.Controllers;
using SistemaEFood.Modelos;
using SistemaEFood.Utilidades;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace PruebasEFood.Tests.Controllers
{
    public class TarjetaControllerTests
    {
        private readonly Mock<IUnidadTrabajo> _mockUnidadTrabajo;
        private readonly TarjetaController tarjetaControllerPrueba;

        public TarjetaControllerTests()
        {
            _mockUnidadTrabajo = new Mock<IUnidadTrabajo>();
            tarjetaControllerPrueba = new TarjetaController(_mockUnidadTrabajo.Object);

            // Simula el login y creación de un usuario
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser")
            }, "mock"));

            // Simula el HTTP necesario para el funcionamiento de la prueba
            tarjetaControllerPrueba.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Simula la existencia del TempData en forma de un Diccionario
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>())
            {
                [DS.Exitosa] = "Success message",
                [DS.Error] = "Error message"
            };
            tarjetaControllerPrueba.TempData = tempData;

            // Mocking methods for IBitacora and IBitacoraError
            _mockUnidadTrabajo.Setup(u => u.Bitacora.RegistrarAccion(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            _mockUnidadTrabajo.Setup(u => u.BitacoraError.RegistrarError(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Act
            var result = tarjetaControllerPrueba.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Upsert_Get_IdEsNulo_RetornaVistaConNuevaTarjeta()
        {
            // Act
            var result = await tarjetaControllerPrueba.Upsert((int?)null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Tarjeta>(viewResult.Model);
            Assert.Equal(0, model.Id);
        }

        [Fact]
        public async Task Upsert_Get_IdNoEsNulo_RetornaNotFoundCuandoTarjetaEsNulo()
        {
            // Arrange
            _mockUnidadTrabajo.Setup(u => u.Tarjeta.Obtener(It.IsAny<int>())).ReturnsAsync((Tarjeta)null);

            // Act
            var result = await tarjetaControllerPrueba.Upsert(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Upsert_Get_IdNoEsNulo_RetornaVistaConTarjetaExistente()
        {
            // Arrange
            var tarjeta = new Tarjeta { Id = 1, Nombre = "Test Tarjeta" };
            _mockUnidadTrabajo.Setup(u => u.Tarjeta.Obtener(It.IsAny<int>())).ReturnsAsync(tarjeta);

            // Act
            var result = await tarjetaControllerPrueba.Upsert(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Tarjeta>(viewResult.Model);
            Assert.Equal(1, model.Id);
            Assert.Equal("Test Tarjeta", model.Nombre);
        }

        [Fact]
        public async Task Upsert_Post_ModelStateEsValido_CreaNuevaTarjeta()
        {
            // Arrange
            var tarjeta = new Tarjeta { Id = 0, Nombre = "Nueva Tarjeta" };
            _mockUnidadTrabajo.Setup(u => u.Tarjeta.Agregar(It.IsAny<Tarjeta>())).Returns(Task.CompletedTask);
            _mockUnidadTrabajo.Setup(u => u.Guardar()).Returns(Task.CompletedTask);

            // Act
            var result = await tarjetaControllerPrueba.Upsert(tarjeta);

            // Assert
            _mockUnidadTrabajo.Verify(u => u.Tarjeta.Agregar(It.Is<Tarjeta>(t => t == tarjeta)), Times.Once);
            _mockUnidadTrabajo.Verify(u => u.Guardar(), Times.Once);
            _mockUnidadTrabajo.Verify(u => u.Bitacora.RegistrarAccion("testuser", It.IsAny<string>()), Times.Once);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(tarjetaControllerPrueba.Index), redirectResult.ActionName);
        }

        [Fact]
        public async Task Upsert_Post_ModelStateEsValido_ActualizaTarjetaExistente()
        {
            // Arrange
            var tarjeta = new Tarjeta { Id = 1, Nombre = "Tarjeta Actualizada" };
            
            _mockUnidadTrabajo.Setup(u => u.Tarjeta.Actualizar(It.IsAny<Tarjeta>()));
            _mockUnidadTrabajo.Setup(u => u.Guardar()).Returns(Task.CompletedTask);

            // Act
            var result = await tarjetaControllerPrueba.Upsert(tarjeta);

            // Assert
            _mockUnidadTrabajo.Verify(u => u.Tarjeta.Actualizar(It.Is<Tarjeta>(t => t == tarjeta)), Times.Once);
            _mockUnidadTrabajo.Verify(u => u.Guardar(), Times.Once);
            _mockUnidadTrabajo.Verify(u => u.Bitacora.RegistrarAccion("testuser", It.IsAny<string>()), Times.Once);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(tarjetaControllerPrueba.Index), redirectResult.ActionName);
        }

        [Fact]
        public async Task Upsert_Post_ModelStateNoEsValido_RetornaVistaConTarjeta()
        {
            // Arrange
            var tarjeta = new Tarjeta { Id = 0, Nombre = "Tarjeta Inválida" };
            tarjetaControllerPrueba.ModelState.AddModelError("Nombre", "El Nombre es requerido");

            // Act
            var result = await tarjetaControllerPrueba.Upsert(tarjeta);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Tarjeta>(viewResult.Model);
            Assert.Equal(tarjeta, model);
            Assert.True(tarjetaControllerPrueba.ModelState.ContainsKey("Nombre"));
        }
    }
}
