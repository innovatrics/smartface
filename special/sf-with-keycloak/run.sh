#!/bin/bash

set -x
set -e

docker network create sf-network || true

# Keycloak container runs as UID 1000 (jboss) and needs to read the realm export
chmod a+r keycloak-server/realm-export.json

docker compose -f keycloak-server/docker-compose.yml up -d
docker compose -f nginx/docker-compose.yml up -d

sleep 10

cd sf-server
./run.sh
