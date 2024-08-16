DB_CONNECTION_STRING="Host=haproxy;Port=5000;Database=library;Username=postgres;Password=postgres"

echo "Migrating database schemas and tables..."

dotnet /migratorapp/Library.Database.Migrator.Psql.dll "$DB_CONNECTION_STRING" "./Scripts/Migrations"

echo "Seeding database..."

dotnet /migratorapp/Library.Database.Migrator.Psql.dll "$DB_CONNECTION_STRING" "./Scripts/Seeds"