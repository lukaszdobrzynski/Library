using Library.Modules.Reservation.Domain.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Modules.Reservation.Infrastructure.Domain.Books;

public class BookEntityTypeConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("books", "reservations");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.LibraryBranchId).HasColumnName("library_branch_id");
        
        builder.OwnsOne<BookCategory>("BookCategory", b =>
        {
            b.Property(x => x.Value).HasColumnName("book_category");
        });
    }
}