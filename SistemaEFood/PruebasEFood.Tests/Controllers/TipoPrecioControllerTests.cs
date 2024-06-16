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

public class TipoPrecioControllerTests
{
    private readonly Mock<IUnidadTrabajo> _mockUnidadTrabajo;
    private readonly TipoPrecioController tipoPrecioControllerPrueba;

    public TipoPrecioControllerTests()
    {
        _mockUnidadTrabajo = new Mock<IUnidadTrabajo>();
        tipoPrecioControllerPrueba = new TipoPrecioController(_mockUnidadTrabajo.Object);
        //Simula el login y creacion de un usuario
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
                new Claim(ClaimTypes.Name, "testuser")
        }, "mock"));

        //Simula el HTTP necesario para el funcionamiento de la prueba
        tipoPrecioControllerPrueba.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };

        //Simula la existencia del TempData en forma de un Diccionario
        var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>())
        {
            [DS.Exitosa] = "Success message",
            [DS.Error] = "Error message"
        };
        tipoPrecioControllerPrueba.TempData = tempData;

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
        var result = tipoPrecioControllerPrueba.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Upsert_Get_IdIsNull_ReturnsViewWithNewTipoPrecio()
    {
        // Act
        var result = await tipoPrecioControllerPrueba.Upsert((int?)null);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<TipoPrecio>(viewResult.Model);
        Assert.Equal(0, model.Id); // New TipoPrecio should have Id == 0
    }

    [Fact]
    public async Task Upsert_Get_IdIsNotNull_ReturnsViewWithTipoPrecio()
    {
        // Arrange
        var tipoPrecioId = 1;
        var tipoPrecio = new TipoPrecio { Id = tipoPrecioId, Nombre = "Test" };
        _mockUnidadTrabajo.Setup(u => u.TipoPrecio.Obtener(tipoPrecioId)).ReturnsAsync(tipoPrecio);

        // Act
        var result = await tipoPrecioControllerPrueba.Upsert(tipoPrecioId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<TipoPrecio>(viewResult.Model);
        Assert.Equal(tipoPrecioId, model.Id);
    }

    [Fact]
    public async Task Upsert_Get_IdIsNotNull_TipoPrecioNotFound_ReturnsNotFound()
    {
        // Arrange
        var tipoPrecioId = 1;
        _mockUnidadTrabajo.Setup(u => u.TipoPrecio.Obtener(tipoPrecioId)).ReturnsAsync((TipoPrecio)null);

        // Act
        var result = await tipoPrecioControllerPrueba.Upsert(tipoPrecioId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Upsert_Post_ModelIsValid_IdIsZero_CreatesTipoPrecio()
    {
        // Arrange
        var tipoPrecio = new TipoPrecio { Id = 0, Nombre = "Nuevo Tipo" };
        _mockUnidadTrabajo.Setup(u => u.TipoPrecio.Agregar(It.IsAny<TipoPrecio>())).Returns(Task.CompletedTask);
        _mockUnidadTrabajo.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
        _mockUnidadTrabajo.Setup(u => u.Bitacora.RegistrarAccion(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

        // Act
        var result = await tipoPrecioControllerPrueba.Upsert(tipoPrecio);

        // Assert
        _mockUnidadTrabajo.Verify(u => u.TipoPrecio.Agregar(It.Is<TipoPrecio>(t => t == tipoPrecio)), Times.Once);
        _mockUnidadTrabajo.Verify(u => u.Guardar(), Times.Once);
        _mockUnidadTrabajo.Verify(u => u.Bitacora.RegistrarAccion(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(tipoPrecioControllerPrueba.Index), redirectResult.ActionName);
    }

    [Fact]
    public async Task Upsert_Post_ModelIsValid_IdIsNotZero_UpdatesTipoPrecio()
    {
        // Arrange
        var tipoPrecio = new TipoPrecio { Id = 1, Nombre = "Actualizado" };
        _mockUnidadTrabajo.Setup(u => u.TipoPrecio.Actualizar(It.IsAny<TipoPrecio>()));
        _mockUnidadTrabajo.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
        _mockUnidadTrabajo.Setup(u => u.Bitacora.RegistrarAccion(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

        // Act
        var result = await tipoPrecioControllerPrueba.Upsert(tipoPrecio);

        // Assert
        _mockUnidadTrabajo.Verify(u => u.TipoPrecio.Actualizar(It.Is<TipoPrecio>(t => t == tipoPrecio)), Times.Once);
        _mockUnidadTrabajo.Verify(u => u.Guardar(), Times.Once);
        _mockUnidadTrabajo.Verify(u => u.Bitacora.RegistrarAccion(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(tipoPrecioControllerPrueba.Index), redirectResult.ActionName);
    }

    [Fact]
    public async Task Upsert_Post_ModelIsNotValid_ReturnsViewWithModel()
    {
        // Arrange
        var tipoPrecio = new TipoPrecio { Id = 1, Nombre = "Test" };
        tipoPrecioControllerPrueba.ModelState.AddModelError("Error", "Model is invalid");

        // Act
        var result = await tipoPrecioControllerPrueba.Upsert(tipoPrecio);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<TipoPrecio>(viewResult.Model);
        Assert.Equal(tipoPrecio.Id, model.Id);
    }
}
