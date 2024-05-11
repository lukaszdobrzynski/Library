using Library.Modules.Reservation.Domain.Patrons;
using Library.Modules.Reservation.Infrastructure.Domain.Patrons;
using Microsoft.EntityFrameworkCore;

namespace Library.Modules.Reservation.Infrastructure;

public class ReservationContext : DbContext
{
    internal DbSet<Patron> Patrons { get; set; }

    public ReservationContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PatronEntityTypeConfiguration());
    }
}