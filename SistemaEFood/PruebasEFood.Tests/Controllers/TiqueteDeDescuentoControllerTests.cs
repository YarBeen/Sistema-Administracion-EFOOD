using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Areas.Admin.Controllers;
using SistemaEFood.Modelos;
using SistemaEFood.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PruebasEFood.Tests.Controllers
{
    public class TiqueteDeDescuentoControllerTests
    {
        private readonly Mock<IUnidadTrabajo> _mockUnidadTrabajo;
        private readonly TiqueteDeDescuentoController tiqueteDeDescuentoControllerPrueba;

        public TiqueteDeDescuentoControllerTests()
        {
            _mockUnidadTrabajo = new Mock<IUnidadTrabajo>();
            tiqueteDeDescuentoControllerPrueba = new TiqueteDeDescuentoController(_mockUnidadTrabajo.Object);

            // Simula el login y creación de un usuario
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser")
            }, "mock"));

            // Simula el HTTP necesario para el funcionamiento de la prueba
            tiqueteDeDescuentoControllerPrueba.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Simula la existencia del TempData en forma de un Diccionario
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>())
            {
                [DS.Exitosa] = "Success message",
                [DS.Error] = "Error message"
            };
            tiqueteDeDescuentoControllerPrueba.TempData = tempData;

            // Mocking methods for IBitacora and IBitacoraError
            _mockUnidadTrabajo.Setup(u => u.Bitacora.RegistrarAccion(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            _mockUnidadTrabajo.Setup(u => u.BitacoraError.RegistrarError(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task Upsert_Get_IdIsNull_ReturnsViewWithNewTiqueteDeDescuento()
        {
            // Act
            var result = await tiqueteDeDescuentoControllerPrueba.Upsert((int?)null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<TiqueteDeDescuento>(viewResult.Model);
            Assert.Equal(0, model.Id);
        }

        [Fact]
        public async Task Upsert_Get_IdIsNotNull_TiqueteDeDescuentoExists_ReturnsViewWithTiqueteDeDescuento()
        {
            // Arrange
            var tiqueteDeDescuento = new TiqueteDeDescuento { Id = 1, Nombre = "descuento1" };
            _mockUnidadTrabajo.Setup(u => u.TiqueteDeDescuento.Obtener(1)).ReturnsAsync(tiqueteDeDescuento);

            // Act
            var result = await tiqueteDeDescuentoControllerPrueba.Upsert(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<TiqueteDeDescuento>(viewResult.Model);
            Assert.Equal(1, model.Id);
            Assert.Equal("descuento1", model.Nombre);
        }

        [Fact]
        public async Task Upsert_Get_IdIsNotNull_TiqueteDeDescuentoDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            _mockUnidadTrabajo.Setup(u => u.TiqueteDeDescuento.Obtener(1)).ReturnsAsync((TiqueteDeDescuento)null);

            // Act
            var result = await tiqueteDeDescuentoControllerPrueba.Upsert(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Upsert_Post_ModelIsValid_IdIsZero_CreatesTiqueteDeDescuento()
        {
            // Arrange
            var tiqueteDeDescuento = new TiqueteDeDescuento { Id = 0, Nombre = "Nuevo Tiquete" };
            _mockUnidadTrabajo.Setup(u => u.TiqueteDeDescuento.Agregar(It.IsAny<TiqueteDeDescuento>())).Returns(Task.CompletedTask);
            _mockUnidadTrabajo.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _mockUnidadTrabajo.Setup(u => u.Bitacora.RegistrarAccion(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            var result = await tiqueteDeDescuentoControllerPrueba.Upsert(tiqueteDeDescuento);

            // Assert
            _mockUnidadTrabajo.Verify(u => u.TiqueteDeDescuento.Agregar(It.Is<TiqueteDeDescuento>(t => t == tiqueteDeDescuento)), Times.Once);
            _mockUnidadTrabajo.Verify(u => u.Guardar(), Times.Once);
            _mockUnidadTrabajo.Verify(u => u.Bitacora.RegistrarAccion("testuser", It.IsAny<string>()), Times.Once);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(tiqueteDeDescuentoControllerPrueba.Index), redirectResult.ActionName);
        }

        [Fact]
        public async Task Upsert_Post_ModelIsValid_IdIsNotZero_UpdatesTiqueteDeDescuento()
        {
            // Arrange
            var tiqueteDeDescuento = new TiqueteDeDescuento { Id = 1, Nombre = "Actualizar Tiquete" };
            _mockUnidadTrabajo.Setup(u => u.TiqueteDeDescuento.Actualizar(It.IsAny<TiqueteDeDescuento>()));
            _mockUnidadTrabajo.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _mockUnidadTrabajo.Setup(u => u.Bitacora.RegistrarAccion(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            var result = await tiqueteDeDescuentoControllerPrueba.Upsert(tiqueteDeDescuento);

            // Assert
            _mockUnidadTrabajo.Verify(u => u.TiqueteDeDescuento.Actualizar(It.Is<TiqueteDeDescuento>(t => t == tiqueteDeDescuento)), Times.Once);
            _mockUnidadTrabajo.Verify(u => u.Guardar(), Times.Once);
            _mockUnidadTrabajo.Verify(u => u.Bitacora.RegistrarAccion("testuser", It.IsAny<string>()), Times.Once);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(tiqueteDeDescuentoControllerPrueba.Index), redirectResult.ActionName);
        }

        [Fact]
        public async Task Upsert_Post_ModelIsNotValid_ReturnsViewWithModel()
        {
            // Arrange
            var tiqueteDeDescuento = new TiqueteDeDescuento { Id = 1, Nombre = "Invalid Tiquete" };
            tiqueteDeDescuentoControllerPrueba.ModelState.AddModelError("Error", "Model is invalid");

            // Act
            var result = await tiqueteDeDescuentoControllerPrueba.Upsert(tiqueteDeDescuento);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<TiqueteDeDescuento>(viewResult.Model);
            Assert.Equal(tiqueteDeDescuento, model);
        }
    }
}
