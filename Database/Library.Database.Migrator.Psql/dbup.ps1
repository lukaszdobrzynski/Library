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

Write-Host "Cluster created successfully."

docker exec -it $CONTAINER_ONE repmgr -f /etc/repmgr.conf cluster show

Write-Host "Verifying PgPool connections..."

docker exec -it pgpool pg_isready -h $CONTAINER_ONE -p 5432 -U postgres
docker exec -it pgpool pg_isready -h $CONTAINER_TWO -p 5432 -U postgres
docker exec -it pgpool pg_isready -h $CONTAINER_THREE -p 5432 -U postgres

Write-Host "Starting SSH services..."

docker exec -it -u root $CONTAINER_ONE service ssh start > $null
docker exec -it -u root $CONTAINER_TWO service ssh start > $null
docker exec -it -u root $CONTAINER_THREE service ssh start > $null
docker exec -it -u root pgpool service ssh start > $null

Write-Host "Adding SSH known hosts..."

docker exec -it -u root pgpool bash -c "ssh-keyscan -H {'$CONTAINER_ONE','$CONTAINER_TWO','$CONTAINER_THREE'} | tee -a /root/.ssh/known_hosts" > $null

Write-Host ""

exit 0