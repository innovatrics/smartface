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
$COMPOSE_COMMAND docker-compose.yml up -d