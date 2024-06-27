using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Modules.Reservation.Infrastructure.InternalCommands;

public class InternalCommandTypeConfiguration : IEntityTypeConfiguration<InternalCommand>
{
    public void Configure(EntityTypeBuilder<InternalCommand> builder)
    {
        builder.ToTable("internal_commands", "reservations");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Data).HasColumnName("data");
        builder.Property(x => x.Data).HasColumnType("json");
        builder.Property(x => x.Type).HasColumnName("type");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.ProcessedAt).HasColumnName("processed_at");
        builder.Property(x => x.ProcessingError).HasColumnName("processing_error");
    }
}