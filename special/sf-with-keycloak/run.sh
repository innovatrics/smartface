#!/bin/bash

set -x
set -e

docker network create sf-network || true

docker compose -f keycloak-server/docker-compose.yml up -d
docker compose -f nginx/docker-compose.yml up -d

sleep 10

cd sf-server
./run.sh
