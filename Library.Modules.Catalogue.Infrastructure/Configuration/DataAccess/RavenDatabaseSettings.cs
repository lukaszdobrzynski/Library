namespace Library.Modules.Catalogue.Infrastructure.Configuration.DataAccess;

public class RavenDatabaseSettings
{
    public string DatabaseName { get; set; }
    public string[] Urls { get; set; }
    public string CertificatePath { get; set; }
    public string CertificatePassword { get; set; }
    public int? RequestTimeoutInSeconds { get; set; }
}