using Library.Modules.Catalogue.Infrastructure.Configuration.DataAccess;
using Library.Modules.Reservation.Infrastructure.Configuration.DataAccess;

namespace Library.API;

public class Settings
{
    public RavenSettings Raven { get; set; }
    public PostgresSettings Postgres { get; set; }
}