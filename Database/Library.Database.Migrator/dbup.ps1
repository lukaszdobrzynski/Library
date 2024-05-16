$imageName = "library"
$containerName = "library"
$dbConnectionString = "Host=localhost;Port=5432;Database=library;Username=postgres;Password=admin"
$hostPort = 5432
$containerPort = 5432

docker build -t $imageName .

if ($LASTEXITCODE -eq 0) {
    Write-Host "Docker image '$imageName' built successfully."
    Write-Host "Running $containerName container..."
    
    docker run -d --rm -p "${hostPort}:${containerPort}" --name $containerName $imageName

    if ($LASTEXITCODE -eq 0) {
        Write-Host "Docker container $containerName started successfully."
    } else
    {
        Write-Host "Failed to start Docker container $containerName." -ForegroundColor Red
        exit -1
    }
    
    Write-Host "Building Library database migrator tool app..."
    
    dotnet build

    if ($LASTEXITCODE -eq 0) {
        Write-Host "Library migrator app built successfully."
    } else
    {
        Write-Host "Failed to build Library migrator app." -ForegroundColor Red
        exit -1
    }
    
    Write-Host "Migrating database schemas and tables..."
    
    dotnet run $dbConnectionString ".\Scripts\Migrations"
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Successfully migrated database schemas and tables."
    } 
    else {
        Write-Host "Failed to migrate database schemas and tables." -ForegroundColor Red
        exit -1
    }
    
    Write-Host "Seeding database..."

    dotnet run $dbConnectionString ".\Scripts\Seeds"

    if ($LASTEXITCODE -eq 0) {
        Write-Host "Database seeded successfully."
    }
    else {
        Write-Host "Failed to seed database." -ForegroundColor Red
        exit -1
    }
    
    Write-Host "Library database ready and listening on port $hostPort."
    exit 0
    
} else {
    Write-Host "Failed to build Docker image '$imageName'." -ForegroundColor Red
}