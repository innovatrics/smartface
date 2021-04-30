#!/bin/bash

set -x
set -e

if [ ! -f iengine.lic ]; then
    echo "License file not found. Please make sure that the license file is present in the current directory."
    exit 1
fi

# sf-network is used so that sf-dependencies and sf containers can communicate
# this can fail if the network already exists, but we don't mind that
docker network create sf-network || true

# start dependencies of SF - MsSql, RMQ and minio
docker-compose -f sf_dependencies/docker-compose.yml up -d

# sleep to wait for the dependencies to start up
sleep 10

# load version and registry from .env
VERSION=$(grep SF_VERSION .env | cut -d '=' -f2 | cut -d$'\r' -f1)
REGISTRY=$(grep REGISTRY .env | cut -d '=' -f2 | cut -d$'\r' -f1)

echo $VERSION
echo $REGISTRY

# create SmartFace database in PgSql
docker exec pgsql psql -U postgres -c "CREATE DATABASE smartface"
# run database migration to current version
docker run --rm --name admin_migration --network sf-network ${REGISTRY}sf-jetson-admin:${VERSION} run-migration -p 5 -c "Server=pgsql;Database=smartface;Username=postgres;Password=Test1234;Pooling=False;Trust Server Certificate=true;" -dbe PgSql

# finally start SF images
docker-compose -f jetson-docker-compose.yml up -d --force-recreate