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
    public class ProcesadorDePagoConfiguracion : IEntityTypeConfiguration<ProcesadorDePago>
    {
        public void Configure(EntityTypeBuilder<ProcesadorDePago> builder)
        {
            builder.Property(X => X.Id).IsRequired();
            builder.Property(X => X.Procesador).IsRequired().HasMaxLength(40);
            builder.Property(X => X.NombreOpcionDePago).IsRequired().HasMaxLength(40);
            builder.Property(X => X.Tipo).IsRequired().HasMaxLength(40);
            builder.Property(X => X.Estado).IsRequired();
            builder.Property(X => X.Verificacion).IsRequired();
            builder.Property(X => X.Metodo).IsRequired().HasMaxLength(40);

            //Many to many
            builder.HasMany(X => X.Tarjetas).WithMany(Y => Y.ProcesadoresDePagos).UsingEntity(J => J.ToTable("TarjetaProcesador")); //.OnDelete(DeleteBehavior.NoAction);


        }
    }
}
