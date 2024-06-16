using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using SistemaEFood.AccesoDatos.Repositorio.IRepositorio;
using SistemaEFood.Areas.Admin.Controllers;
using SistemaEFood.Modelos;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Drawing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SistemaEFood.Utilidades;

namespace MyProject.Tests.Controllers
{
    public class ProcesadorDePagoControllerTests
    {
        private readonly Mock<IUnidadTrabajo> _mockUnidadTrabajo;
        private readonly ProcesadorDePagoController procesadorDePagoControllerPrueba;

        public ProcesadorDePagoControllerTests()
        {
            _mockUnidadTrabajo = new Mock<IUnidadTrabajo>();
            procesadorDePagoControllerPrueba = new ProcesadorDePagoController(_mockUnidadTrabajo.Object);

            //Simula el login y creacion de un usuario
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser")
            }, "mock"));

            //Simula el HTTP necesario para el funcionamiento de la prueba
            procesadorDePagoControllerPrueba.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            //Simula la existencia del TempData en forma de un Diccionario
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>())
            {
                [DS.Exitosa] = "Success message",
                [DS.Error] = "Error message"
            };
            procesadorDePagoControllerPrueba.TempData = tempData;

            // Mocking methods for IBitacora and IBitacoraError
            _mockUnidadTrabajo.Setup(u => u.Bitacora.RegistrarAccion(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            _mockUnidadTrabajo.Setup(u => u.BitacoraError.RegistrarError(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);
        }

        [Fact]
        //Descripcion: Retorna un valor nulo
        public async Task Index_IdNula_RetornaViewResultConNullModel()
        {
            // Arrange
            _mockUnidadTrabajo.Setup(u => u.ProcesadorDePago.Obtener(null)).ReturnsAsync((ProcesadorDePago)null);

            // Act
            var result = await procesadorDePagoControllerPrueba.Index(null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.Model);
        }

        [Fact]
        //Descripcion: Retorna un valor no nulo, es decir, con una id que sí exista dentro de la entidad
        public async Task Index_IdNoNula_RetornaViewResultConProcesadorDePago()
        {
            //Arrange
            var procesadorDePago = new ProcesadorDePago { Id = 1 };
            _mockUnidadTrabajo.Setup(u => u.ProcesadorDePago.Obtener(1)).ReturnsAsync(procesadorDePago);

            // Act
            var result = await procesadorDePagoControllerPrueba.Index(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ProcesadorDePago>(viewResult.Model);
            Assert.Equal(1, model.Id);
        }
        [Fact]
        //Metodo Upsert
        //Upsert cuando la ID de entrada es nula
        public async Task Upsert_IdNula_RetornaViewResultConNuevoModelo()
        {
            //Act
            var result = await procesadorDePagoControllerPrueba.Upsert((int?)null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ProcesadorDePago>(viewResult.Model);
            Assert.NotNull(model);
            Assert.Equal(0, model.Id); // Verifica que es un modelo nuevo
        }
        [Fact]
        //Cuando el ID no es nulo, da una entrada y espera el retorno de un modelo cuya ID es la misma de la entrada
        public async Task Upsert_IdNoNulo_EncuentraModel_RetornaViewResultConModeloEncontrado()
        {
            //Arrange
            var procesadorDePago = new ProcesadorDePago { Id = 1 };
            _mockUnidadTrabajo.Setup(u => u.ProcesadorDePago.Obtener(1)).ReturnsAsync(procesadorDePago);

            // Act
            var result = await procesadorDePagoControllerPrueba.Upsert((int?)1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ProcesadorDePago>(viewResult.Model);
            Assert.Equal(1, model.Id);
        }
        [Fact]
        //Cuando el ID no es nulo, no encuentra un modelo con esa ID y retorna un Not Found
        public async Task Upsert_IdNoNulo_ModeloNoEncontrado_RetornaResultadoNotFound()
        {
            // Arrange
            _mockUnidadTrabajo.Setup(u => u.ProcesadorDePago.Obtener(1)).ReturnsAsync((ProcesadorDePago)null);

            // Act
            var result = await procesadorDePagoControllerPrueba.Upsert((int?)1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        //Metodo UPSERT HTTPOST
        [Fact]
        public async Task Upsert_Post_ModelStateValid_IdEsCero_AddsProcesadorDePago()
        {
            // Arrange
            var procesadorDePago = new ProcesadorDePago { Id = 0, NombreOpcionDePago = "Prueba de Pago" };
            procesadorDePagoControllerPrueba.ModelState.Clear();

            //Remover, SETUP necesario para asegurar que fuera solo la entrada que detecta como nulo
            _mockUnidadTrabajo.Setup(u => u.ProcesadorDePago.Actualizar(It.IsAny<ProcesadorDePago>()));
            _mockUnidadTrabajo.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _mockUnidadTrabajo.Setup(u => u.Bitacora.RegistrarAccion(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            // Act
            var result = await procesadorDePagoControllerPrueba.Upsert(procesadorDePago);

            // Assert
            _mockUnidadTrabajo.Verify(u => u.ProcesadorDePago.Agregar(procesadorDePago), Times.Once);
            _mockUnidadTrabajo.Verify(u => u.Guardar(), Times.Once);
            _mockUnidadTrabajo.Verify(u => u.Bitacora.RegistrarAccion("testuser", It.IsAny<string>()), Times.Once);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(procesadorDePagoControllerPrueba.Index), redirectResult.ActionName);
        }
        [Fact]
        public async Task Upsert_Post_ModelStateValid_IdNoEsCero_UpdatesProcesadorDePago()
        {
            // Arrange
            var procesadorDePago = new ProcesadorDePago
            {
                Id = 1,
                Procesador = "Procesador Test",
                NombreOpcionDePago = "Opción Test",
                Tipo = "Tipo Test",
                Estado = true,
                Verificacion = true,
                Metodo = "Metodo Test"
            };
            //Remover, SETUP necesario para asegurar que fuera solo la entrada que detecta como nulo
            _mockUnidadTrabajo.Setup(u => u.ProcesadorDePago.Actualizar(It.IsAny<ProcesadorDePago>()));
            _mockUnidadTrabajo.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _mockUnidadTrabajo.Setup(u => u.Bitacora.RegistrarAccion(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            var result = await procesadorDePagoControllerPrueba.Upsert(procesadorDePago);

            // Assert
            _mockUnidadTrabajo.Verify(u => u.ProcesadorDePago.Actualizar(procesadorDePago), Times.Once);
            _mockUnidadTrabajo.Verify(u => u.Guardar(), Times.Once);
            _mockUnidadTrabajo.Verify(u => u.Bitacora.RegistrarAccion("testuser", It.IsAny<string>()), Times.Once);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(procesadorDePagoControllerPrueba.Index), redirectResult.ActionName);
        }
    }
}
