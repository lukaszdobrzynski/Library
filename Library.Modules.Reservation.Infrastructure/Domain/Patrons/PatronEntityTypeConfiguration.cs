using Library.Modules.Reservation.Domain.Patrons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Modules.Reservation.Infrastructure.Domain.Patrons;

public class PatronEntityTypeConfiguration : IEntityTypeConfiguration<Patron>
{
    public void Configure(EntityTypeBuilder<Patron> builder)
    {
        builder.ToTable("patrons", "reservations");

        builder.HasKey(x => x.Id);
        builder.Property(p => p.Id).HasColumnName("id");

        builder.OwnsOne<PatronType>("_patronType", b =>
        {
            b.Property(p => p.Type).HasColumnName("patron_type");
        });
    }
}