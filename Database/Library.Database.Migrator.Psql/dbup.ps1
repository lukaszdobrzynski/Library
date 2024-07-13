$dbConnectionString = "Host=localhost;Port=5432;Database=library;Username=postgres;Password=admin"

$CONTAINER_ONE="postgres-node-one"
$CONTAINER_TWO="postgres-node-two"
$CONTAINER_THREE="postgres-node-three"

Write-Host "Running compose command..."

docker compose up -d

if ($LASTEXITCODE -eq 0)
{
    Write-Host "Docker compose services started successfully."
} else
{
    Write-Host "Failed to start Docker compose services." -ForegroundColor Red
    exit -1
}

Write-Host "Building Library database migrator tool app..."

dotnet build ./Library.Database.Migrator.Psql.csproj

if ($LASTEXITCODE -eq 0) {
    Write-Host "Migrator app built successfully."
} else
{
    Write-Host "Failed to build migrator app." -ForegroundColor Red
    exit -1
}

Write-Host "Migrating database schemas and tables..."

dotnet run --project ./Library.Database.Migrator.Psql.csproj $dbConnectionString ".\Scripts\Migrations"

if ($LASTEXITCODE -eq 0) {
    Write-Host "Successfully migrated database schemas and tables."
}
else {
    Write-Host "Failed to migrate database schemas and tables." -ForegroundColor Red
    exit -1
}

Write-Host "Seeding database..."

dotnet run --project ./Library.Database.Migrator.Psql.csproj $dbConnectionString ".\Scripts\Seeds"

if ($LASTEXITCODE -eq 0) {
    Write-Host "Database seeded successfully."
}
else {
    Write-Host "Failed to seed database." -ForegroundColor Red
    exit -1
}

Write-Host "Registering the primary cluster server..."

docker exec -it $CONTAINER_ONE repmgr -f /etc/repmgr.conf primary register

if ($LASTEXITCODE -eq 0) {
    Write-Host "Primary cluster server registered successfully."
}
else {
    Write-Host "Failed to register primary cluster server." -ForegroundColor Red
    exit -1
}

Write-Host "Modifying primary cluster server access permissions..."

$CONTAINER_TWO_IP=$(docker inspect -f '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' $CONTAINER_TWO)
$CONTAINER_THREE_IP=$(docker inspect -f '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' $CONTAINER_THREE)

docker exec -it $CONTAINER_ONE sh -c "echo 'host replication postgres $CONTAINER_TWO_IP/32 trust' >> /var/lib/postgresql/data/pg_hba.conf"
$SET_PRIMARYDB_ACCESS_PERMISSION_FOR_CONTAINER_TWO_EXIT_CODE = $LASTEXITCODE
docker exec -it $CONTAINER_ONE sh -c "echo 'host replication postgres $CONTAINER_THREE_IP/32 trust' >> /var/lib/postgresql/data/pg_hba.conf"
$SET_PRIMARYDB_ACCESS_PERMISSION_FOR_CONTAINER_THREE_EXIT_CODE = $LASTEXITCODE

if ($SET_PRIMARYDB_ACCESS_PERMISSION_FOR_CONTAINER_TWO_EXIT_CODE -eq 0) {
    Write-Host "Access permission for $CONTAINER_TWO set on primary database server."
}
else {
    Write-Host "Failed to set access permission for $CONTAINER_TWO on the primary database server." -ForegroundColor Red
    exit -1
}

if ($SET_PRIMARYDB_ACCESS_PERMISSION_FOR_CONTAINER_THREE_EXIT_CODE -eq 0) {
    Write-Host "Access permission for $CONTAINER_THREE set on primary database instance."
}
else {
    Write-Host "Failed to set access permission for $CONTAINER_THREE on the primary database instance." -ForegroundColor Red
    exit -1
}

Write-Host "Reloading primary db instance..."

docker exec -it $CONTAINER_ONE pg_ctl reload -D /var/lib/postgresql/data

if ($LASTEXITCODE -eq 0) {
    Write-Host "Primary database instance reloaded."
}
else {
    Write-Host "Failed to reload primary database instance." -ForegroundColor Red
    exit -1
}

Write-Host "Registering $CONTAINER_TWO database instance as a standby..."

docker exec -it $CONTAINER_TWO repmgr -h $CONTAINER_ONE -U postgres -d library -f /etc/repmgr.conf standby clone
$CLONE_PRIMARY_TO_STANDBY_ONE_EXIT_CODE = $LASTEXITCODE
docker exec -it $CONTAINER_TWO pg_ctl -D /var/lib/postgresql/data start
$RESTART_STANDBY_ONE_EXIT_CODE = $LASTEXITCODE
docker exec -it $CONTAINER_TWO repmgr -f /etc/repmgr.conf standby register
$REGISTER_STANDBY_ONE_EXIT_CODE = $LASTEXITCODE

if ($CLONE_PRIMARY_TO_STANDBY_ONE_EXIT_CODE -eq 0) {
    Write-Host "Cloned primary instance to standby one instance."
}
else {
    Write-Host "Failed to clone primary instance to standby one." -ForegroundColor Red
    exit -1
}

if ($RESTART_STANDBY_ONE_EXIT_CODE -eq 0) {
    Write-Host "Standby one instance restarted successfully."
}
else {
    Write-Host "Failed to restart standby one instance." -ForegroundColor Red
    exit -1
}

if ($REGISTER_STANDBY_ONE_EXIT_CODE -eq 0) {
    Write-Host "Standby one instance registered successfully."
}
else {
    Write-Host "Failed to register standby one instance." -ForegroundColor Red
    exit -1
}


Write-Host "Registering $CONTAINER_THREE database instance as a standby..."

docker exec -it $CONTAINER_THREE repmgr -h $CONTAINER_ONE -U postgres -d library -f /etc/repmgr.conf standby clone
$CLONE_PRIMARY_TO_STANDBY_TWO_EXIT_CODE = $LASTEXITCODE
docker exec -it $CONTAINER_THREE pg_ctl -D /var/lib/postgresql/data start
$RESTART_STANDBY_TWO_EXIT_CODE = $LASTEXITCODE
docker exec -it $CONTAINER_THREE repmgr -f /etc/repmgr.conf standby register
$REGISTER_STANDBY_TWO_EXIT_CODE = $LASTEXITCODE

if ($CLONE_PRIMARY_TO_STANDBY_TWO_EXIT_CODE -eq 0) {
    Write-Host "Cloned primary instance to standby two instance."
}
else {
    Write-Host "Failed to clone primary instance to standby two." -ForegroundColor Red
    exit -1
}

if ($RESTART_STANDBY_TWO_EXIT_CODE -eq 0) {
    Write-Host "Standby two instance restarted successfully."
}
else {
    Write-Host "Failed to restart standby two instance." -ForegroundColor Red
    exit -1
}

if ($REGISTER_STANDBY_TWO_EXIT_CODE -eq 0) {
    Write-Host "Standby two instance registered successfully."
}
else {
    Write-Host "Failed to register standby two instance." -ForegroundColor Red
    exit -1
}


Write-Host "Cluster created successfully."

docker exec -it $CONTAINER_ONE repmgr -f /etc/repmgr.conf cluster show
exit 0