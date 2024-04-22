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
    public class TipoPrecioConfiguracion : IEntityTypeConfiguration<TipoPrecio>
    {
        public void Configure(EntityTypeBuilder<TipoPrecio> builder)
        {
            builder.Property(X => X.Id).IsRequired();
            builder.Property(X => X.Nombre).IsRequired().HasMaxLength(40);
            
        }
    }
}
