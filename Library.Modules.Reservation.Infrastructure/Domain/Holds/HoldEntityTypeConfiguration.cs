using Library.Modules.Reservation.Domain.Holds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Modules.Reservation.Infrastructure.Domain.Holds;

public class HoldEntityTypeConfiguration : IEntityTypeConfiguration<Hold>
{
    public void Configure(EntityTypeBuilder<Hold> builder)
    {
        builder.ToTable("holds", "reservations");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.LibraryBranchId).HasColumnName("library_branch_id");
        builder.Property(x => x.BookId).HasColumnName("book_id");
        builder.Property(x => x.PatronId).HasColumnName("patron_id");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Ignore(x => x.DomainEvents);

        builder.OwnsOne<HoldPeriod>("Period", b =>
        {
            b.Property(p => p.Value).HasColumnName("period");
        });
    }
}