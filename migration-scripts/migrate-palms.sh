#!/bin/bash

getvalue() {
    local key="$1"
    local value=$(grep -E ^${key}= .env | cut -d '=' -f2- | cut -d$'\r' -f1)
    echo "$value"
}

# load version and registry from .env
VERSION="$(getvalue SF_VERSION)"
REGISTRY="$(getvalue REGISTRY)"

SF_ADMIN_IMAGE=${REGISTRY}sf-admin:${VERSION}
SF_PALM_DETECTOR_IMAGE=${REGISTRY}sf-palm-detector:${VERSION}
SF_PALM_EXTRACTOR_IMAGE=${REGISTRY}sf-palm-extractor:${VERSION}

# check docker compose command presence
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

# stop all services before migration
$COMPOSE_COMMAND down --remove-orphans

# Set the number of palm detector containers to run (default is 3)
PALM_DETECTORS_COUNT=${PALM_DETECTORS_COUNT:-3}
PALM_EXTRACTORS_COUNT=${PALM_EXTRACTORS_COUNT:-3}

# Stop all possible previously containers with names prefixed by sf_migration_
for c in $(docker ps -aq --filter "name=^/sf_migration_"); do
  docker stop "$c" 2>/dev/null || true
done

echo "Spawning palm detector and extractor containers for migration"

for i in $(seq 1 $PALM_DETECTORS_COUNT)
do
  docker run --rm --name sf_migration_palm_detector_$i \
    --env RabbitMQ__Hostname="$(getvalue RabbitMQ__Hostname)" \
    --env RabbitMQ__Username="$(getvalue RabbitMQ__Username)" \
    --env RabbitMQ__Password="$(getvalue RabbitMQ__Password)" \
    --env RabbitMQ__Port="$(getvalue RabbitMQ__Port)" \
    --volume $(pwd)/iengine.lic:/etc/innovatrics/iengine.lic \
    --network sf-network \
    ${SF_PALM_DETECTOR_IMAGE}
done

for i in $(seq 1 $PALM_EXTRACTORS_COUNT)
do
  docker run --rm --name sf_migration_palm_extractor_$i \
    --env RabbitMQ__Hostname="$(getvalue RabbitMQ__Hostname)" \
    --env RabbitMQ__Username="$(getvalue RabbitMQ__Username)" \
    --env RabbitMQ__Password="$(getvalue RabbitMQ__Password)" \
    --env RabbitMQ__Port="$(getvalue RabbitMQ__Port)" \
    --volume $(pwd)/iengine.lic:/etc/innovatrics/iengine.lic \
    --network sf-network \
    ${SF_PALM_EXTRACTOR_IMAGE}
done

echo "Calling migrate-palms command to migrate palms"

# RUN PALMS MIGRATION
docker run --rm --name sf_admin \
  --volume "$(pwd)/iengine.lic:/etc/innovatrics/iengine.lic" \
  --network sf-network \
  "${SF_ADMIN_IMAGE}" \
  migrate-palms \
  -c "$(getvalue ConnectionStrings__CoreDbContext)" \
  -dbe "$(getvalue Database__DbEngine)" \
  -rmq-host "$(getvalue RabbitMQ__Hostname)" \
  -rmq-user "$(getvalue RabbitMQ__Username)" \
  -rmq-pass "$(getvalue RabbitMQ__Password)" \
  -rmq-vhost "$(getvalue RabbitMQ__VirtualHost)" \
  -rmq-p "$(getvalue RabbitMQ__Port)" \
  -rmq-use-ssl "$(getvalue RabbitMQ__UseSsl)" \
  -s3-e "$(getvalue S3__Endpoint)" \
  -s3-bn "$(getvalue S3__BucketName)" \
  -s3-ak "$(getvalue S3__AccessKey)" \
  -s3-sk "$(getvalue S3__SecretKey)" \
  -s3-f "$(getvalue S3__Folder)" \
  --parallelism 4

# Stop all containers with names prefixed by sf_migration_
for c in $(docker ps -aq --filter "name=^/sf_migration_"); do
  docker stop "$c" 2>/dev/null || true
done

echo "Calling set-state-error-non-migrated-palms command with dry-run to see what was not migrated successfully"

# RUN DRY-RUN OF SET STATE ERROR FOR MIGRATED PALMS
docker run --rm --name sf_admin \
    --volume "$(pwd)/iengine.lic:/etc/innovatrics/iengine.lic" \
    --network sf-network \
    "${SF_ADMIN_IMAGE}" \
    set-state-error-non-migrated-palms \
    -c "$(getvalue ConnectionStrings__CoreDbContext)" \
    -dbe "$(getvalue Database__DbEngine)" \
    --dry-run

echo "To finalize migration for some palms that cannot be migrated, run script ./finalize-non-migrated-palms.sh"
echo "WARNING: Do this only if you are confident that the palms cannot be migrated and you want to mark them as error state"
echo "WARNING: If there were some transient erros like RPC timeouts or other errors, do not finalize the migration and try to migrate again"