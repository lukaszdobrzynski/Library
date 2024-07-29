CONTAINER_ONE=postgres-node-one
CONTAINER_TWO=postgres-node-two
CONTAINER_FOUR_IP=172.199.99.5

initdb -D /var/lib/postgresql/data

echo "host all all 172.199.99.3/32 trust" >> /var/lib/postgresql/data/pg_hba.conf
sed -i "1s/^/host all postgres $CONTAINER_FOUR_IP\/32 trust\n/" /var/lib/postgresql/data/pg_hba.conf

pg_ctl -D /var/lib/postgresql/data start

while ! pg_isready -U postgres; do
  echo "PostgreSQL is not ready yet. Waiting..."
  sleep 2
done

psql -U postgres -c "CREATE DATABASE library;"

pg_ctl -D /var/lib/postgresql/data stop

repmgr -h $CONTAINER_ONE -U postgres -d library -f /etc/repmgr.conf standby clone -F

pg_ctl -D /var/lib/postgresql/data start

repmgr -f /etc/repmgr.conf standby register

pg_ctl -D /var/lib/postgresql/data stop
postgres -D /var/lib/postgresql/data \
  -c wal_level=replica \
  -c hot_standby=on \
  -c max_wal_senders=10 \
  -c max_replication_slots=10 \
  -c hot_standby_feedback=on