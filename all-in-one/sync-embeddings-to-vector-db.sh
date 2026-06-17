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

# Vector database endpoint to populate. Defaults to the bundled Milvus service.
VECTOR_DB_ENDPOINT=${VECTOR_DB_ENDPOINT:-http://milvus:19530}

# Number of watchlist members loaded per SQL database paging batch.
BATCH_SIZE=${BATCH_SIZE:-1000}

# Optional behaviour toggles (set the env variable to "true" to enable).
# DRY_RUN       - verify the connections and report what would be synced without writing anything.
# FORCE_REWRITE - drop and recreate the embedding collections before indexing (destructive, required
#                 when the collections already exist).
OPTIONAL_ARGS=()
if [ "${DRY_RUN}" = "true" ]; then
  OPTIONAL_ARGS+=(--dry-run)
fi
if [ "${FORCE_REWRITE}" = "true" ]; then
  OPTIONAL_ARGS+=(--force-rewrite)
fi

# Ensure the Milvus vector database is running before syncing.
echo "Starting Milvus vector database"
docker compose -f sf_dependencies/docker-compose.yml up -d milvus-etcd milvus

echo "Calling sync-embeddings-to-vector-db command to sync face and palm embeddings from the SQL database to the vector database"

docker run --rm --name sf_admin \
  --network sf-network \
  "${SF_ADMIN_IMAGE}" \
  sync-embeddings-to-vector-db \
  -c "$(getvalue ConnectionStrings__CoreDbContext)" \
  -dbe "$(getvalue Database__DbEngine)" \
  --vector-db-endpoint "${VECTOR_DB_ENDPOINT}" \
  --batch-size "${BATCH_SIZE}" \
  "${OPTIONAL_ARGS[@]}"

echo "Done"
