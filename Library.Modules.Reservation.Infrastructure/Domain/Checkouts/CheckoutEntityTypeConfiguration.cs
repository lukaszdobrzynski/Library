using Library.Modules.Reservation.Domain.Checkouts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Modules.Reservation.Infrastructure.Domain.Checkouts;

public class CheckoutEntityTypeConfiguration : IEntityTypeConfiguration<Checkout>
{
    public void Configure(EntityTypeBuilder<Checkout> builder)
    {
        builder.ToTable("checkouts", "reservations");

        builder.HasKey(x => x.Id);
        builder.Property(p => p.Id).HasColumnName("id");
        builder.Property(p => p.LibraryBranchId).HasColumnName("library_branch_id");
        builder.Property(p => p.BookId).HasColumnName("book_id");
        builder.Property(p => p.PatronId).HasColumnName("patron_id");
        builder.Property(p => p.DueDate).HasColumnName("due_date");
        builder.Property(p => p.VersionId).HasColumnName("version_id").IsConcurrencyToken();
    }
}