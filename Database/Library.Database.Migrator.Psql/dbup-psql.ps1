Write-Host "Running compose command..."

docker compose up -d

if ($LASTEXITCODE -eq 0)
{
    Write-Host "Patroni cluster created successfully."
    exit 0
} else
{
    Write-Host "Failed to start Docker compose services." -ForegroundColor Red
    exit -1
}