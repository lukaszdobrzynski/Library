$currentDir = Get-Location
$licenseDir = "developer-license"

$fullLicensePath = Join-Path -Path $currentDir -ChildPath $licenseDir
$env:LICENSE_PATH = $fullLicensePath

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

Write-Host "Adding nodes to cluster..."

Start-Sleep -Seconds 5

$response = curl -s -S -w "%{response_code}" -X PUT http://127.0.0.1:8080/admin/cluster/node?url=http://library-catalogue-two:8081
$addNode2StatusCode = $response.Substring($response.Length - 3)
$responseBody = $response.Substring(0, $response.Length - 3)

if ($LASTEXITCODE -ne 0) {
    Write-Host "Curl command to add node 2 failed." -ForegroundColor Red
    Write-Host "Error details: $responseBody" -ForegroundColor Red
    exit -1
}

if ($addNode2StatusCode -eq 204) {
    Write-Host "Node 2 added successfully with server response status code ${addNode2StatusCode}."
} else {
    Write-Host "Failed to add node 2 with server response status code ${addNode2StatusCode}." -ForegroundColor Red
    Write-Host "Server returned error details: $responseBody" -ForegroundColor Red
    exit -1
}

$response = curl -s -S -w "%{response_code}" -X PUT http://127.0.0.1:8080/admin/cluster/node?url=http://library-catalogue-three:8082
$addNode3StatusCode = $response.Substring($response.Length - 3)
$responseBody = $response.Substring(0, $response.Length - 3)

if ($LASTEXITCODE -ne 0) {
    Write-Host "Curl command to add node 3 failed." -ForegroundColor Red
    Write-Host "Error details: $responseBody" -ForegroundColor Red
    exit -1
}

if ($addNode3StatusCode -eq 204) {
    Write-Host "Node 3 added successfully with server response status code ${addNode3StatusCode}."
} else {
    Write-Host "Failed to add node 3 with server response status code ${addNode3StatusCode}." -ForegroundColor Red
    Write-Host "Server returned error details: $responseBody" -ForegroundColor Red
    exit -1
}

Write-Host "Creating database..."

Start-Sleep -Seconds 5

$statusCode = curl -o NUL -w "%{response_code}" -X PUT -d @init-db.json -H 'Content-Type:application/json' http://127.0.0.1:8080/admin/databases

if ($LASTEXITCODE -ne 0) {
    Write-Host "Curl command to create a database failed." -ForegroundColor Red
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

Write-Host "Building database migrator...."

dotnet build

if ($LASTEXITCODE -eq 0) {
    Write-Host "Library migrator app built successfully."
} else
{
    Write-Host "Failed to build Library migrator app." -ForegroundColor Red
    exit -1
}

Write-Host "Seeding database..."

Start-Sleep -Seconds 5

dotnet run

if ($LASTEXITCODE -eq 0) {
    Write-Host "Database seeded successfuly."
    exit 0
}
else {
    Write-Host "Failed to seed database." -ForegroundColor Red
    exit -1
}