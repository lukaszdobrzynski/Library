chmod +x ./var/pgpool-failover.sh

echo "host all all 172.199.99.5/32 trust" >> /etc/pgpool2/pool_hba.conf

pgpool -n