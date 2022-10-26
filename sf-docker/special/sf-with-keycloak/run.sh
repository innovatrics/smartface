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
        echo "No compose command found. Please install docker compose" >&2
        exit 1
    fi
fi

set -e

# start dependencies of SF - MsSql, RMQ and minio
$COMPOSE_COMMAND -f keycloak-server/docker-compose.yml up -d

sleep 10

cd sf-Server
./run.sh