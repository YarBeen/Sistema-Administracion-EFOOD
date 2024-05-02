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
    public class ProcesadorTarjetaConfiguracion : IEntityTypeConfiguration<ProcesadorTarjeta>
    {
        public void Configure(EntityTypeBuilder<ProcesadorTarjeta> builder)
        {
            builder.Property(X => X.Id).IsRequired();
            builder.Property(X => X.ProcesadorId).IsRequired();
            builder.Property(X => X.TarjetaId).IsRequired();



            /* Relaciones */
            builder.HasOne(x => x.Tarjeta).WithMany()
                .HasForeignKey(x => x.TarjetaId).
                OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.ProcesadorDePago).WithMany()
                .HasForeignKey(x => x.ProcesadorId).
                OnDelete(DeleteBehavior.NoAction);


        }
    }
}
