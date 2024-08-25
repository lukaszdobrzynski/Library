using System;

namespace Library.Modules.Reservation.IntegrationTests;

public class TestSettings
{
    public const string Username = "postgres";
    public const string Password = "postgres";
    public const string Hostname = "localhost";
    public static string DbConnectionString(int hostPort, Guid dbName) => $"Host={Hostname};Port={hostPort};Database={dbName};Username={Username};Password={Password};IncludeErrorDetail=True";
}