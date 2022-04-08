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

# create SmartFace database in MsSql
docker exec mssql /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Test1234 -Q "CREATE DATABASE SmartFace" || true

# load version and registry from .env
VERSION=$(grep -E ^SF_VERSION .env | cut -d '=' -f2 | cut -d$'\r' -f1)
REGISTRY=$(grep -E ^REGISTRY .env | cut -d '=' -f2 | cut -d$'\r' -f1)

# load RabbitMQ properties from .env
RMQ_HOST=$(grep -E ^RabbitMQ__Hostname .env | cut -d '=' -f2 | cut -d$'\r' -f1)
RMQ_USER=$(grep -E ^RabbitMQ__Username .env | cut -d '=' -f2 | cut -d$'\r' -f1)
RMQ_PASS=$(grep -E ^RabbitMQ__Password .env | cut -d '=' -f2 | cut -d$'\r' -f1)
RMQ_VHOST=$(grep -E ^RabbitMQ__VirtualHost .env | cut -d '=' -f2 | cut -d$'\r' -f1)
RMQ_PORT=$(grep -E ^RabbitMQ__Port .env | cut -d '=' -f2 | cut -d$'\r' -f1)
RMQ_SSL=$(grep -E ^RabbitMQ__UseSsl .env | cut -d '=' -f2 | cut -d$'\r' -f1)

# run database migration to current version
docker run --rm --name admin_migration --network sf-network ${REGISTRY}sf-admin:${VERSION} run-migration -p 5 -c "Server=mssql;Database=SmartFace;User ID=sa;Password=Test1234;" --rmq-host ${RMQ_HOST} --rmq-user ${RMQ_USER} --rmq-pass ${RMQ_PASS} --rmq-virtual-host ${RMQ_VHOST} --rmq-port ${RMQ_PORT} --rmq-use-ssl ${RMQ_SSL}

# finally start SF images
docker-compose -f cloud-matcher-docker-compose.yml up -d
