using Library.Modules.Reservation.Application.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Modules.Reservation.Infrastructure.Outbox;

public class OutboxEntityTypeConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages", "reservations");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Data).HasColumnName("data");
        builder.Property(x => x.Data).HasColumnType("json");
        builder.Property(x => x.Type).HasColumnName("type");
        builder.Property(x => x.OccurredOn).HasColumnName("occurred_on");
        builder.Property(x => x.ProcessedAt).HasColumnName("processed_at");
    }
}