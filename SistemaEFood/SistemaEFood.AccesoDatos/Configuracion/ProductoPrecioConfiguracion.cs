using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaEFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEFood.AccesoDatos.Configuracion
{
    public class ProductoPrecioConfiguracion : IEntityTypeConfiguration<ProductoPrecio>
    {
        public void Configure(EntityTypeBuilder<ProductoPrecio> builder)
        {
            builder.Property(X => X.Id).IsRequired();
            builder.Property(X => X.Monto);
            builder.Property(X => X.Idprecio).IsRequired();
            builder.Property(X => X.Idproducto).IsRequired();
        }
    }
}
