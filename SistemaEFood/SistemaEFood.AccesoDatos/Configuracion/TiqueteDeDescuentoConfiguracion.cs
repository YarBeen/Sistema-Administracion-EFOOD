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
    public class TiqueteDeDescuentoConfiguracion : IEntityTypeConfiguration<TiqueteDeDescuento>
    {
        public void Configure(EntityTypeBuilder<TiqueteDeDescuento> builder)
        {
            builder.Property(X => X.Id).IsRequired();
            builder.Property(X => X.Codigo).IsRequired().HasMaxLength(40);
            builder.Property(X => X.Nombre).IsRequired().HasMaxLength(40);
            builder.Property(X => X.Disponibles).IsRequired();
            builder.Property(X => X.Descuento).IsRequired();


        }
    }
}
