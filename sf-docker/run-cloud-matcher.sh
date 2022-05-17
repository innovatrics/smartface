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

# start dependencies of SF - MsSql, RMQ and minio
$COMPOSE_COMMAND -f sf_dependencies/docker-compose.yml up -d

# sleep to wait for the dependencies to start up
sleep 10

# create SmartFace database in MsSql
docker exec mssql /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Test1234 -Q "CREATE DATABASE SmartFace" || true

# load version and registry from .env
VERSION=$(grep -E ^SF_VERSION .env | cut -d '=' -f2 | cut -d$'\r' -f1)
REGISTRY=$(grep -E ^REGISTRY .env | cut -d '=' -f2 | cut -d$'\r' -f1)

# run database migration to current version
docker run --rm --name admin_migration --network sf-network ${REGISTRY}sf-admin:${VERSION} run-migration -p 5 -c "Server=mssql;Database=SmartFace;User ID=sa;Password=Test1234;"

# finally start SF images
$COMPOSE_COMMAND -f cloud-matcher-docker-compose.yml up -d
