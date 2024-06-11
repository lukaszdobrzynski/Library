$imageName = "library-catalogue"
$containerName = "library-catalogue"
$hostPort1 = 8080
$containerPort1 = 8080
$hostPort2 = 38888
$containerPort2 = 38888


docker build -t $imageName .

if ($LASTEXITCODE -eq 0) {
    Write-Host "Docker image '$imageName' built successfully."
    Write-Host "Running $containerName container..."

    docker run -d --rm -p "${hostPort1}:${containerPort1}" -p "${hostPort2}:${containerPort2}" --name $containerName $imageName

    if ($LASTEXITCODE -eq 0) 
    {
        Write-Host "Docker container $containerName started successfully."
    } else
    {
        Write-Host "Failed to start Docker container $containerName." -ForegroundColor Red
        exit -1
    }

    Write-Host "Creating database..."

    Start-Sleep -Seconds 5

    $statusCode = curl -o NUL -w "%{response_code}" -X PUT -d @init-db.json -H 'Content-Type:application/json' http://127.0.0.1:8080/admin/databases

    if ($LASTEXITCODE -ne 0) {
        Write-Host "Curl command failed." -ForegroundColor Red
        exit -1
    }

    if ($statusCode -eq 201)
    {
        Write-Host "Database created successfully with server response status code ${statusCode}."
        
    } else
    {
        Write-Host "Failed to create database with response status code ${statusCode}." -ForegroundColor Red
        exit -1
    }

    dotnet build

    if ($LASTEXITCODE -eq 0) {
        Write-Host "Library migrator app built successfully."
    } else
    {
        Write-Host "Failed to build Library migrator app." -ForegroundColor Red
        exit -1
    }

    Write-Host "Seeding database..."

    dotnet run

    if ($LASTEXITCODE -eq 0) {
        Write-Host "Database seeded successfuly."
        exit 0
    }
    else {
        Write-Host "Failed to seed database." -ForegroundColor Red
        exit -1
    }

} else {
    Write-Host "Failed to build Docker image '$imageName'." -ForegroundColor Red
    exit -1
}