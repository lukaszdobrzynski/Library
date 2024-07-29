DB_CONNECTION_STRING="Host=localhost;Port=5432;Database=library;Username=postgres;Password=admin"

CONTAINER_TWO_IP=172.199.99.3
CONTAINER_THREE_IP=172.199.99.4
CONTAINER_FOUR_IP=172.199.99.5

initdb -D /var/lib/postgresql/data

echo "Modifying primary server access permissions..."
echo "host all all 172.199.99.1/32 trust" >> /var/lib/postgresql/data/pg_hba.conf
echo "host all all 172.199.99.6/32 trust" >> /var/lib/postgresql/data/pg_hba.conf
echo "host library postgres 172.199.99.2/32 trust" >> /var/lib/postgresql/data/pg_hba.conf
echo "host library postgres 172.199.99.3/32 trust" >> /var/lib/postgresql/data/pg_hba.conf
echo "host library postgres 172.199.99.4/32 trust" >> /var/lib/postgresql/data/pg_hba.conf
echo "host replication postgres $CONTAINER_TWO_IP/32 trust" >> /var/lib/postgresql/data/pg_hba.conf
echo "host replication postgres $CONTAINER_THREE_IP/32 trust" >> /var/lib/postgresql/data/pg_hba.conf
sed -i "1s/^/host all postgres $CONTAINER_FOUR_IP\/32 trust\n/" /var/lib/postgresql/data/pg_hba.conf

pg_ctl -D /var/lib/postgresql/data start

while ! pg_isready -U postgres; do
  echo "PostgreSQL is not ready yet. Waiting..."
  sleep 2
done

psql -U postgres -c "CREATE DATABASE library;"

repmgr -f /etc/repmgr.conf primary register

pg_ctl -D /var/lib/postgresql/data stop

postgres -D /var/lib/postgresql/data \
  -c wal_level=replica \
  -c hot_standby=on \
  -c max_wal_senders=10 \
  -c max_replication_slots=10 \
  -c hot_standby_feedback=on