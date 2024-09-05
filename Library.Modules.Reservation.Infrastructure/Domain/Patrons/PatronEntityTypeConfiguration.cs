using Library.Modules.Reservation.Domain.Patrons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Modules.Reservation.Infrastructure.Domain.Patrons;

internal class PatronEntityTypeConfiguration : IEntityTypeConfiguration<Patron>
{
    public void Configure(EntityTypeBuilder<Patron> builder)
    {
        builder.ToTable("patrons", "reservations");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.VersionId).HasColumnName("version_id").IsConcurrencyToken();
        builder.Ignore(x => x.DomainEvents);
        
        builder.OwnsOne<PatronType>("_patronType", b =>
        {
            b.Property(p => p.Type).HasColumnName("patron_type");
        });
    }
}