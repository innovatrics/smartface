set -e
# HighAvailabilityClusterNetwork is used so that sf-dependencies and sf containers can communicate
# this can fail if the network already exists, but we don't mind that
docker network create HighAvailabilityClusterNetwork || true

# start dependencies of SF - MsSql, RMQ and minio
$COMPOSE_COMMAND up -d

# sleep to wait for the dependencies to start up
sleep 10

# to switch DB engine, change the .env file
DB_ENGINE=$(grep -E ^Database__DbEngine .env | cut -d '=' -f2 | cut -d$'\r' -f1)

echo $VERSION
echo $REGISTRY

# create SmartFace database in PgSql
docker exec pgsql psql -U postgres -c "CREATE DATABASE smartface" || true
# run database migration to current version
docker run --rm --name admin_migration --network HighAvailabilityClusterNetwork ${REGISTRY}sf-admin:${VERSION} run-migration -p 5 -c "Server=pgsql;Database=smartface;Username=postgres;Password=Test1234;Trust Server Certificate=true;" -dbe $DB_ENGINE --rmq-host ${RMQ_HOST} --rmq-user ${RMQ_USER} --rmq-pass ${RMQ_PASS} --rmq-virtual-host ${RMQ_VHOST} --rmq-port ${RMQ_PORT} --rmq-use-ssl ${RMQ_SSL}