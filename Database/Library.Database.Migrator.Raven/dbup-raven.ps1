Write-Host "Running compose command..."

docker compose up -d

if ($LASTEXITCODE -eq 0) 
{
    Write-Host "Docker compose services started successfully."
    exit 0
} else
{
    Write-Host "Failed to start Docker compose services." -ForegroundColor Red
    exit -1
}