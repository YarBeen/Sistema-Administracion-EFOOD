using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFoodVistaCliente.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFoodVistaCliente.AccesoDatos.Configuracion
{
    public class ProductoConfiguracion : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.Property(X => X.Id).IsRequired();
            builder.Property(X => X.Contenido).IsRequired().HasMaxLength(100);
            builder.Property(X => X.Nombre).IsRequired().HasMaxLength(40);
            builder.Property(X => X.LineaComidaId).IsRequired();
            builder.Property(X => X.ImagenUrl).IsRequired(false);
            builder.Property(X => X.PadreId).IsRequired(false);

            /* Relaciones */

            builder.HasOne(x => x.LineaComida).WithMany()
                .HasForeignKey(x => x.LineaComidaId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Padre).WithMany()
                .HasForeignKey(x => x.PadreId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
