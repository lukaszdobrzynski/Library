using Library.Modules.Reservation.Application.Outbox;
using Library.Modules.Reservation.Domain.Books;
using Library.Modules.Reservation.Domain.Checkouts;
using Library.Modules.Reservation.Domain.Holds;
using Library.Modules.Reservation.Domain.Patrons;
using Library.Modules.Reservation.Infrastructure.Domain.Books;
using Library.Modules.Reservation.Infrastructure.Domain.Checkouts;
using Library.Modules.Reservation.Infrastructure.Domain.Holds;
using Library.Modules.Reservation.Infrastructure.Domain.Patrons;
using Library.Modules.Reservation.Infrastructure.InternalCommands;
using Library.Modules.Reservation.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Library.Modules.Reservation.Infrastructure;

internal class ReservationContext : DbContext
{
    internal DbSet<Patron> Patrons { get; set; }
    internal DbSet<Book> Books { get; set; }
    internal DbSet<Hold> Holds { get; set; }
    internal DbSet<OutboxMessage> OutboxMessages { get; set; }
    internal DbSet<Checkout> Checkouts { get; set; }
    internal DbSet<InternalCommand> InternalCommands { get; set; }

    public ReservationContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PatronEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BookEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new HoldEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CheckoutEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InternalCommandTypeConfiguration());
    }
}