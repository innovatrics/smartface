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
# docker network create sf-observability || true
docker network create sf-network || true

$COMPOSE_COMMAND up -d

# sleep to wait for the dependencies to start up
sleep 3

# create mqtt user for rmq mqtt plugin
# docker run --rm --network sf-observability --entrypoint /bin/sh minio/mc -c "/usr/bin/mc config host add obsminio http://obsminio:9000 minioadmin minioadmin"
# docker run --rm --network sf-observability --entrypoint /bin/sh minio/mc -c "/usr/bin/mc mb obsminio/loki"
# #docker run --rm --network sf-observability --entrypoint /bin/sh minio/mc -c "/usr/bin/mc policy set public obsminio/loki"
# docker run --rm --network sf-observability --entrypoint /bin/sh minio/mc -c "/usr/bin/mc mc anonymous set public obsminio/loki"

# docker run --rm --network sf-observability --entrypoint /bin/sh minio/mc -c "
#       /usr/bin/mc config host add obsminio http://obsminio:9000 minioadmin minioadmin;
#       /usr/bin/mc mb obsminio/loki;
#       /usr/bin/mc anonymous set public obsminio/loki;
#       /usr/bin/mc mc ls obsminio;
#       exit 0;
#       "

#docker run --rm --network sf-observability --entrypoint /bin/sh minio/mc -c "/usr/bin/mc mc ls obsminio"

cd ../all-in-one/
./run.sh