using Library.Modules.Reservation.Domain.Patrons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Modules.Reservation.Infrastructure.Domain.Patrons;

public class PatronEntityTypeConfiguration : IEntityTypeConfiguration<Patron>
{
    public void Configure(EntityTypeBuilder<Patron> builder)
    {
        builder.ToTable("Patrons", "reservations");
        
        builder.HasKey(x => x.Id);

        builder.OwnsOne<PatronType>("_patronType", b =>
        {
            b.Property(p => p.Type).HasColumnName("PatronType");
        });
    }
}