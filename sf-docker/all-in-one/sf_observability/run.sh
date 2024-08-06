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