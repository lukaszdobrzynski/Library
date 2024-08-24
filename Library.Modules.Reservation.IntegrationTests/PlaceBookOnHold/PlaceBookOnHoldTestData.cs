using System;

namespace Library.Modules.Reservation.IntegrationTests.PlaceBookOnHold;

public static class PlaceBookOnHoldTestData
{
    public static Guid RegularPatronId { get; } = Guid.Parse("f0e4109e-4975-439a-aa11-d369f8d6a9ec");

    public static Guid CirculatingBookId { get; } = Guid.Parse("a0d44d08-7c55-4b67-9f13-5c11f9d3a1b5");
}