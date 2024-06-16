using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaEFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.AccesoDatos.Configuracion
{
    public class OrdenConfiguracion : IEntityTypeConfiguration<Orden>
    {
        public void Configure(EntityTypeBuilder<Orden> builder)
        {
            builder.Property(X => X.Id).IsRequired();
            builder.Property(X => X.Cliente).IsRequired();
            builder.Property(X => X.FechaOrden).IsRequired();
            builder.Property(X => X.NumeroEnvio).IsRequired(false);
            builder.Property(X => X.TotalOrden).IsRequired();
            builder.Property(X => X.EstadoOrden).IsRequired();
            builder.Property(X => X.Telefono).IsRequired(false);
            builder.Property(X => X.Direccion).IsRequired(false);
            builder.Property(X => X.NombresCliente).IsRequired(false);
            builder.Property(X => X.ApellidosCliente).IsRequired(false);
            builder.Property(X => X.CodigoTiqueteDeDescuento).IsRequired(false);





        }
    }
}
