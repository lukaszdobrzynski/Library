using Library.Modules.Reservation.Domain.Holds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Modules.Reservation.Infrastructure.Domain.Holds;

public class LibraryHoldDecisionEntityTypeConfiguration : IEntityTypeConfiguration<LibraryHoldDecision>
{
    public void Configure(EntityTypeBuilder<LibraryHoldDecision> builder)
    {
        builder.ToTable("library_hold_decisions", "reservations");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.HoldId).HasColumnName("hold_id");
        
        builder.OwnsOne<LibraryHoldDecisionStatus>("DecisionStatus", b =>
        {
            b.Property(p => p.Value).HasColumnName("decision_status");
        });
    }
}