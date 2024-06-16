using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Areas.Admin.Controllers;
using SistemaEFood.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SistemaEFood.Modelos;

namespace PruebasEFood.Tests.Controllers
{
    public class LineaComidaControllerTests
    {
        private readonly Mock<IUnidadTrabajo> _mockUnidadTrabajo;
        private readonly LineaComidaController lineaComidaControllerPrueba;

        public LineaComidaControllerTests() 
        {
            _mockUnidadTrabajo = new Mock<IUnidadTrabajo>();
            lineaComidaControllerPrueba = new LineaComidaController(_mockUnidadTrabajo.Object);
            //Simula el login y creacion de un usuario
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser")
            }, "mock"));

            //Simula el HTTP necesario para el funcionamiento de la prueba
            lineaComidaControllerPrueba.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            //Simula la existencia del TempData en forma de un Diccionario
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>())
            {
                [DS.Exitosa] = "Success message",
                [DS.Error] = "Error message"
            };
            lineaComidaControllerPrueba.TempData = tempData;

            // Mocking methods for IBitacora and IBitacoraError
            _mockUnidadTrabajo.Setup(u => u.Bitacora.RegistrarAccion(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            _mockUnidadTrabajo.Setup(u => u.BitacoraError.RegistrarError(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);
        }
        [Fact]
        //Probar el Index para retornar una vista Index
        public void Index_ReturnsViewResult() 
        {
            //Act 
            var resultado = lineaComidaControllerPrueba.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(resultado);
        }
        [Fact]
        //Retorna una vista con un nuevo objeto tipo LineaComida cuando recibe un ID nulo
        public async Task Upsert_Get_IdEsNulo_RetornaVistaConNuevaLineaComida()
        {
            // Act
            var result = await lineaComidaControllerPrueba.Upsert((int?)null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<LineaComida>(viewResult.Model);
            Assert.Equal(0, model.Id);
        }
        [Fact]
        //Encuentra el ID dado, y devuelve la vista asociada con dicho ID
        public async Task Upsert_Get_IdNoEsNulo_RetornaVistaConLineaComida()
        {
            // Arrange
            var lineaComida = new LineaComida { Id = 1, Nombre = "Linea Test" };
            _mockUnidadTrabajo.Setup(u => u.LineaComida.Obtener(1)).ReturnsAsync(lineaComida);

            // Act
            var result = await lineaComidaControllerPrueba.Upsert(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<LineaComida>(viewResult.Model);
            Assert.Equal(lineaComida.Id, model.Id);
        }
        [Fact]
        //Devuelve el mensaje "Not Found" cuando encuentra un ID no nulo pero que no exista
        public async Task Upsert_Get_IdIsNotNull_ReturnsNotFoundWhenLineaComidaIsNull()
        {
            // Arrange
            _mockUnidadTrabajo.Setup(u => u.LineaComida.Obtener(1)).ReturnsAsync((LineaComida)null);

            // Act
            var result = await lineaComidaControllerPrueba.Upsert(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Upsert_Post_ModelStateValid_IdIsZero_CreatesLineaComida()
        {
            // Arrange
            var lineaComida = new LineaComida
            {
                Id = 0,
                Nombre = "Linea Test"
            };

            _mockUnidadTrabajo.Setup(u => u.LineaComida.Agregar(It.IsAny<LineaComida>())).Returns(Task.CompletedTask);
            _mockUnidadTrabajo.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _mockUnidadTrabajo.Setup(u => u.Bitacora.RegistrarAccion(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            
            // Act
            var result = await lineaComidaControllerPrueba.Upsert(lineaComida);

            // Assert
            _mockUnidadTrabajo.Verify(u => u.LineaComida.Agregar(It.Is<LineaComida>(l => l == lineaComida)), Times.Once);
            _mockUnidadTrabajo.Verify(u => u.Guardar(), Times.Once);
            _mockUnidadTrabajo.Verify(u => u.Bitacora.RegistrarAccion("testuser", It.IsAny<string>()), Times.Once);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(lineaComidaControllerPrueba.Index), redirectResult.ActionName);
        }
    }
}
