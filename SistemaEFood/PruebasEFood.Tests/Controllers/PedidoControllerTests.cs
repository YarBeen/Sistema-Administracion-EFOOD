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
using Microsoft.AspNetCore.Hosting;
using SistemaEFood.Servicios;
using SistemaEFood.Modelos.ViewModels;
using SistemaEFood.Modelos;
using System.Linq.Expressions;

namespace PruebasEFood.Tests.Controllers
{
    public class PedidoControllerTests
    {
        private readonly Mock<IUnidadTrabajo> _mockUnidadTrabajo;
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;
        private readonly Mock<IStorageService> _mockStorageService;
        private readonly PedidoController pedidoControllerPrueba;
        public PedidoControllerTests() 
        {
            _mockUnidadTrabajo = new Mock<IUnidadTrabajo>();
            _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            _mockStorageService = new Mock<IStorageService>();
            pedidoControllerPrueba = new PedidoController(_mockUnidadTrabajo.Object, _mockWebHostEnvironment.Object, _mockStorageService.Object);
        }
        [Fact]
        public void Index_RetornaVista()
        {
            // Act
            var result = pedidoControllerPrueba.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        
    }
}

    

