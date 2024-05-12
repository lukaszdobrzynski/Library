using DbUp;
using DbUp.ScriptProviders;

namespace Library.Database.Migrator;

class Program
{
    static int Main(string[] args)
    {
        if (args.Length != 2)
        {
             Console.WriteLine("Incorrect number of arguments: [connectionString] [scriptsPath].");
             return -1;
        }
        
        var connectionString = args[0];
        var scriptsPath = args[1];

        EnsureDatabase.For.PostgresqlDatabase(connectionString);

        var builder = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .LogToConsole()
            .WithScriptsFromFileSystem(scriptsPath, new FileSystemScriptOptions
            {
                IncludeSubDirectories = true
            })
            .WithTransaction();

        builder.Configure(config =>
        {
            config.Journal = new CustomTableJournal(() => 
                    config.ConnectionManager, 
                () => config.Log, "app",
                "migrations_journal");
        });

        var result = builder.Build()
            .PerformUpgrade();

        if (result.Successful == false)
        {
            Console.WriteLine("DB migration failed.");
            return -1;
        }
        
        Console.WriteLine("DB migration performed successfully.");

        return 0;
    }
}