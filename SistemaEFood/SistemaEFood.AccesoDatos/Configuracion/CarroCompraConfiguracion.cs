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
    public class CarroCompraConfiguracion : IEntityTypeConfiguration<CarroCompra>
    {
        public void Configure(EntityTypeBuilder<CarroCompra> builder)
        {
            builder.Property(X => X.Id).IsRequired();
            builder.Property(X => X.ProductoId).IsRequired();
            builder.Property(X => X.Cantidad).IsRequired();


         
            builder.HasOne(x => x.Producto).WithMany()
                .HasForeignKey(x => x.ProductoId).
                OnDelete(DeleteBehavior.NoAction).OnDelete(DeleteBehavior.NoAction);
          

        }
    }
}
