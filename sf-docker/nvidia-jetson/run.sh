#!/bin/bash

set -x
set -e

if [ ! -f iengine.lic ]; then
    echo "License file not found. Please make sure that the license file is present in the current directory." >&2
    exit 1
fi

COMPOSE_COMMAND="docker compose"

set +e

$COMPOSE_COMMAND version

if [ $? -ne 0 ]; then
    COMPOSE_COMMAND="docker-compose"
    $COMPOSE_COMMAND version
    if [ $? -ne 0 ]; then
        echo "No compose command found. Please install docker compose" >&2
        exit 1
    fi
fi

set -e

# sf-network is used so that sf-dependencies and sf containers can communicate
# this can fail if the network already exists, but we don't mind that
docker network create sf-network || true

# start dependencies of SF - PgSql, RMQ and minio
chmod go+rx sf_dependencies/etc_rmq
chmod go+r sf_dependencies/etc_rmq/*
$COMPOSE_COMMAND -f sf_dependencies/docker-compose.yml up -d

# sleep to wait for the dependencies to start up
sleep 30

getvalue() {
    local key="$1"
    local value=$(grep -E ^${key}= .env | cut -d '=' -f2- | cut -d$'\r' -f1)
    echo "$value"
}

# load version and registry from .env
VERSION="$(getvalue SF_VERSION)"
REGISTRY="$(getvalue REGISTRY)"

SF_ADMIN_IMAGE=${REGISTRY}sf-jetson-admin:${VERSION}

# we use the DB engine that will be used by SF to create and migrate the DB
# to switch DB engine, change the .env file
DB_ENGINE="$(getvalue Database__DbEngine)"

# set correct hostname to sfstation env file
sed -i "s/S3_PUBLIC_ENDPOINT=.*/S3_PUBLIC_ENDPOINT=http:\/\/$(hostname):9000/g" .env.sfstation

echo $VERSION
echo $REGISTRY

# create mqtt user for rmq mqtt plugin
docker exec -it rmq /opt/rabbitmq/sbin/rabbitmqctl add_user mqtt mqtt || true
docker exec -it rmq /opt/rabbitmq/sbin/rabbitmqctl set_user_tags mqtt administrator || true
docker exec -it rmq /opt/rabbitmq/sbin/rabbitmqctl set_permissions -p "/" mqtt ".*" ".*" ".*" || true

# create SmartFace database in PgSql
docker exec pgsql psql -U postgres -c "CREATE DATABASE smartface" || true
# run database migration to current version
    docker run --rm --name admin_migration --volume $(pwd)/iengine.lic:/etc/innovatrics/iengine.lic --network sf-network ${SF_ADMIN_IMAGE} \
        run-migration \
            -p "$(getvalue CameraServicesCount)" \
            -c "$(getvalue ConnectionStrings__CoreDbContext)" -dbe $DB_ENGINE \
            --rmq-host "$(getvalue RabbitMQ__Hostname)" --rmq-user "$(getvalue RabbitMQ__Username)" --rmq-pass "$(getvalue RabbitMQ__Password)" \
            --rmq-virtual-host "$(getvalue RabbitMQ__VirtualHost)" --rmq-port "$(getvalue RabbitMQ__Port)" --rmq-use-ssl "$(getvalue RabbitMQ__UseSsl)"

docker run --rm --name s3-bucket-create --network sf-network ${SF_ADMIN_IMAGE} \
    ensure-s3-bucket-exists --endpoint "$(getvalue S3Bucket__Endpoint)" --access-key "$(getvalue S3Bucket__AccessKey)" --secret-key  "$(getvalue S3Bucket__SecretKey)" --bucket-name "$(getvalue S3Bucket__BucketName)"

# finally start SF images
$COMPOSE_COMMAND -f docker-compose.yml up -d --force-recreate