using Library.BuildingBlocks.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Library.Modules.Reservation.Infrastructure;

internal class TypedIdValueConverter<TTypedIdValue> : ValueConverter<TTypedIdValue, Guid>
    where TTypedIdValue : TypedIdBase
{
    public TypedIdValueConverter(ConverterMappingHints mappingHints = null)
        : base(id => id.Value, value => Create(value), mappingHints)
    {
    }

    private static TTypedIdValue Create(Guid id) => Activator.CreateInstance(typeof(TTypedIdValue), id) as TTypedIdValue;
}