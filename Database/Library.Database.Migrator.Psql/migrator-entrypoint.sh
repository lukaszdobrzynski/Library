DB_CONNECTION_STRING="Host=postgres-node-one;Port=5432;Database=library;Username=postgres;Password=admin"

echo "Migrating database schemas and tables..."

dotnet /migratorapp/Library.Database.Migrator.Psql.dll "$DB_CONNECTION_STRING" "./Scripts/Migrations"

echo "Seeding database..."

dotnet /migratorapp/Library.Database.Migrator.Psql.dll "$DB_CONNECTION_STRING" "./Scripts/Seeds"