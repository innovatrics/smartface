#!/bin/bash

set -x
set -e

docker compose -f keycloak-server/docker-compose.yml up -d

sleep 10

cd sf-server
./run.sh