using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaEFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Configuracion
{
    public class OrdenDetalleConfiguracion : IEntityTypeConfiguration<OrdenDetalle>
    {
        public void Configure(EntityTypeBuilder<OrdenDetalle> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.OrdenId).IsRequired();
            builder.Property(x => x.Estado).IsRequired(); ;
            builder.Property(x => x.Medio);
            builder.Property(x => x.Monto).IsRequired();
            builder.Property(x => x.Tipo);
            builder.Property(x => x.ChequeNumero);
            builder.Property(x => x.ChequeCuenta);
            builder.Property(x => x.FechaOrden);

            builder.HasOne(x => x.Orden).WithMany()
                   .HasForeignKey(x => x.OrdenId)
                   .OnDelete(DeleteBehavior.NoAction);

           

        }
    }
}