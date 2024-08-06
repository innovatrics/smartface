#!/bin/bash

set -x
set -e

COMPOSE_COMMAND="docker compose"

set +e

$COMPOSE_COMMAND version

if [ $? -ne 0 ]; then
    COMPOSE_COMMAND="docker-compose"
    $COMPOSE_COMMAND version
    if [ $? -ne 0 ]; then
        error_exit "No compose command found. Please install docker compose"
    fi
fi

set -e

# sf-network is used so that sf-dependencies and sf containers can communicate
# this can fail if the network already exists, but we don't mind that
docker network create sf-network || true

$COMPOSE_COMMAND docker-compose.yml up -d

# sleep to wait for the dependencies to start up
sleep 15

# create mqtt user for rmq mqtt plugin
docker run --rm --network observability --entrypoint /bin/sh minio/mc -c "/usr/bin/mc config host add obsminio http://obsminio:9000 minioadmin minioadmin;"
docker run --rm --network observability --entrypoint /bin/sh minio/mc -c "/usr/bin/mc mb obsminio/loki;"
docker run --rm --network observability --entrypoint /bin/sh minio/mc -c "/usr/bin/mc policy set public obsminio/loki; exit 0;"

# stop smartface core services before migration
$COMPOSE_COMMAND down --remove-orphans

if [[ "$DB_ENGINE" == "MsSql" ]]; then
    # create SmartFace database in MsSql
    docker exec mssql /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Test1234 -Q "CREATE DATABASE SmartFace" || true
    # run database migration to current version
    docker run --rm --name admin_migration --volume $(pwd)/iengine.lic:/etc/innovatrics/iengine.lic --network sf-network ${SF_ADMIN_IMAGE} \
        run-migration \
            -p "$(getvalue CameraServicesCount)" \
            -c "$(getvalue ConnectionStrings__CoreDbContext)" -dbe $DB_ENGINE \
            --tenant-id default \
            --rmq-host "$(getvalue RabbitMQ__Hostname)" --rmq-user "$(getvalue RabbitMQ__Username)" --rmq-pass "$(getvalue RabbitMQ__Password)" \
            --rmq-virtual-host "$(getvalue RabbitMQ__VirtualHost)" --rmq-port "$(getvalue RabbitMQ__Port)" --rmq-streams-port "$(getvalue RabbitMQ__StreamsPort)" --rmq-use-ssl "$(getvalue RabbitMQ__UseSsl)" \
            --dependencies-availability-timeout 120
elif [[ "$DB_ENGINE" == "PgSql" ]]; then
    # create SmartFace database in PgSql
    docker exec pgsql psql -U postgres -c "CREATE DATABASE smartface" || true
    # run database migration to current version
    docker run --rm --name admin_migration --volume $(pwd)/iengine.lic:/etc/innovatrics/iengine.lic --network sf-network ${SF_ADMIN_IMAGE} \
        run-migration \
            -p "$(getvalue CameraServicesCount)" \
            -c "$(getvalue ConnectionStrings__CoreDbContext)" -dbe $DB_ENGINE \
            --tenant-id default \
            --rmq-host "$(getvalue RabbitMQ__Hostname)" --rmq-user "$(getvalue RabbitMQ__Username)" --rmq-pass "$(getvalue RabbitMQ__Password)" \
            --rmq-virtual-host "$(getvalue RabbitMQ__VirtualHost)" --rmq-port "$(getvalue RabbitMQ__Port)" --rmq-streams-port "$(getvalue RabbitMQ__StreamsPort)" --rmq-use-ssl "$(getvalue RabbitMQ__UseSsl)" \
            --dependencies-availability-timeout 120
else
    error_exit "Unknown DB engine: ${DB_ENGINE}!"
fi

docker run --rm --name s3-bucket-create --network sf-network ${SF_ADMIN_IMAGE} \
    ensure-s3-bucket-exists --endpoint "$(getvalue S3Bucket__Endpoint)" --access-key "$(getvalue S3Bucket__AccessKey)" --secret-key  "$(getvalue S3Bucket__SecretKey)" --bucket-name "$(getvalue S3Bucket__BucketName)"

############### NOTE ###############
# Uncomment line below if you are interested in watchlists synchronization from SmartFace platform to edge cameras
#./create-wl-stream-generation.sh

# finally start SF images
$COMPOSE_COMMAND up -d --force-recreate