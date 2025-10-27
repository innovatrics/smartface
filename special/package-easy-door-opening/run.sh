#!/bin/bash

set -x
set -e

function error_exit {
    echo "$1" 1>&2
    exit 1
}

function ensure_docker_version_is_sufficient () {
    requiredMajor=20
    requiredMinor=10
    requiredPatch=10

    # Check if Docker is installed
    if ! command -v docker &> /dev/null; then
        error_exit "Docker is not installed on this machine."
    fi

    actualDockerVersion=$(docker version --format '{{.Server.Version}}')
    if [[ -z "$actualDockerVersion" ]]; then
        error_exit "Unable to determine Docker server version."
    fi
    
    read actualMajor actualMinor actualPatch <<< $( echo ${actualDockerVersion} | awk -F"." '{print $1" "$2" "$3}' )
    
    if [ "$actualMajor" -lt "$requiredMajor" ]; then
        error_exit "Old version of docker detected. Please update your docker to version $requiredMajor.$requiredMinor.$requiredPatch or newer."
    fi

    if [ "$actualMajor" -eq "$requiredMajor" ]; then
        if [ "$actualMinor" -lt "$requiredMinor" ]; then
            error_exit "Old version of docker detected. Please update your docker to version $requiredMajor.$requiredMinor.$requiredPatch or newer."
        fi
        
        if [ "$actualMinor" -eq "$requiredMinor" ]; then
            if [ "$actualPatch" -lt "$requiredPatch" ]; then
                error_exit "Old version of docker detected. Please update your docker to version $requiredMajor.$requiredMinor.$requiredPatch or newer."
            fi
        fi
    fi

    echo "Docker server version is $actualDockerVersion and it meets the requirement."
}

ensure_docker_version_is_sufficient

if [ ! -f iengine.lic ]; then
    error_exit "License file not found. Please make sure that the license file is present in the current directory."
fi

# sf-network is used so that sf-dependencies and sf containers can communicate
# this can fail if the network already exists, but we don't mind that
docker network create sf-network || true

# start dependencies of SF - PgSql, RMQ and minio
chmod go+rx sf_dependencies/etc_rmq
chmod go+r sf_dependencies/etc_rmq/*
docker compose -f sf_dependencies/docker-compose.yml up -d

# sleep to wait for the dependencies to start up
sleep 10

getvalue() {
    local key="$1"
    local value=$(grep -E ^${key}= .env | cut -d '=' -f2- | cut -d$'\r' -f1)
    echo "$value"
}

# load version and registry from .env
VERSION="$(getvalue SF_VERSION)"
REGISTRY="$(getvalue REGISTRY)"

SF_ADMIN_IMAGE=${REGISTRY}sf-admin:${VERSION}

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

# stop smartface core services before migration
docker compose down --remove-orphans

if [[ "$DB_ENGINE" == "MsSql" ]]; then
    # create SmartFace database in MsSql
    docker run --rm --network sf-network mcr.microsoft.com/mssql-tools /opt/mssql-tools/bin/sqlcmd -S mssql -U sa -P Test1234 -Q "CREATE DATABASE SmartFace" || true
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
docker compose up -d --force-recreate
