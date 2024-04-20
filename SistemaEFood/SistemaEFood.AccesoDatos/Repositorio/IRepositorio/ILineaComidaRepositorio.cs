using SistemaEFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface ILineaComidaRepositorio : IRepositorio<LineaComida>
    {
        void Actualizar(LineaComida lineaComida);

    }
}
