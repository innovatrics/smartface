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
VERSION=$(grep -E ^JETSON_VERSION .env | cut -d '=' -f2 | cut -d$'\r' -f1)
REGISTRY=$(grep -E ^REGISTRY .env | cut -d '=' -f2 | cut -d$'\r' -f1)

# load RabbitMQ properties from .env
RMQ_HOST=$(grep -E ^RabbitMQ__Hostname .env | cut -d '=' -f2 | cut -d$'\r' -f1)
RMQ_USER=$(grep -E ^RabbitMQ__Username .env | cut -d '=' -f2 | cut -d$'\r' -f1)
RMQ_PASS=$(grep -E ^RabbitMQ__Password .env | cut -d '=' -f2 | cut -d$'\r' -f1)
RMQ_VHOST=$(grep -E ^RabbitMQ__VirtualHost .env | cut -d '=' -f2 | cut -d$'\r' -f1)
RMQ_PORT=$(grep -E ^RabbitMQ__Port .env | cut -d '=' -f2 | cut -d$'\r' -f1)
RMQ_SSL=$(grep -E ^RabbitMQ__UseSsl .env | cut -d '=' -f2 | cut -d$'\r' -f1)

echo $VERSION
echo $REGISTRY

# create SmartFace database in PgSql
docker exec pgsql psql -U postgres -c "CREATE DATABASE smartface" || true
# run database migration to current version
docker run --rm --name admin_migration --network sf-network ${REGISTRY}sf-jetson-admin:${VERSION} run-migration -p 5 -c "Server=pgsql;Database=smartface;Username=postgres;Password=Test1234;Trust Server Certificate=true;" -dbe PgSql --rmq-host ${RMQ_HOST} --rmq-user ${RMQ_USER} --rmq-pass ${RMQ_PASS} --rmq-virtual-host ${RMQ_VHOST} --rmq-port ${RMQ_PORT} --rmq-use-ssl ${RMQ_SSL}

# finally start SF images
docker-compose -f jetson-docker-compose.yml up -d --force-recreate