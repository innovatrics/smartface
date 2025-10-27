#!/bin/bash

getvalue() {
    local key="$1"
    local value=$(grep -E ^${key}= .env | cut -d '=' -f2- | cut -d$'\r' -f1)
    echo "$value"
}

# load version and registry from .env
SF_VERSION="$(getvalue SF_VERSION)"
REGISTRY="$(getvalue REGISTRY)"

SF_ADMIN_IMAGE=${REGISTRY}sf-admin:${SF_VERSION}
SF_PALM_DETECTOR_IMAGE=${REGISTRY}sf-palm-detector:${SF_VERSION}
SF_PALM_EXTRACTOR_IMAGE=${REGISTRY}sf-palm-extractor:${SF_VERSION}

# stop all services before migration
docker compose down --remove-orphans

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
  echo "Spawning palm detector container"

  docker run -d --rm --name sf_migration_palm_detector_$i \
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
  echo "Spawning palm extractor container"

  docker run -d --rm --name sf_migration_palm_extractor_$i \
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
  -s3-e "$(getvalue S3Bucket__Endpoint)" \
  -s3-bn "$(getvalue S3Bucket__BucketName)" \
  -s3-ak "$(getvalue S3Bucket__AccessKey)" \
  -s3-sk "$(getvalue S3Bucket__SecretKey)" \
  -s3-f "$(getvalue S3Bucket__Folder)" \
  --parallelism 4

echo "Stopping spawned containers"

# Stop all containers with names prefixed by sf_migration_ in parallel and wait for all of them to finish
docker ps -aq --filter "name=^/sf_migration_" | xargs -r -P 0 -n 1 docker stop 2>/dev/null || true

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

echo "WARNING: If there were some transient erros like RPC timeouts or other errors, do not finalize the migration and try to migrate again"
echo "To finalize migration for palms that cannot be migrated, run script ./finalize-non-migrated-palms.sh"