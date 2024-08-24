namespace Library.Modules.Reservation.IntegrationTests;

public class TestSettings
{
    public const string DbName = "library";
    public const string Username = "postgres";
    public const string Password = "postgres";
    public const int HostPort = 5432;
    public const string Hostname = "localhost";
    public static string DbConnectionString => $"Host={Hostname};Port={HostPort};Database={DbName};Username={Username};Password={Password};IncludeErrorDetail=True";
}