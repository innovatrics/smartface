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

# Any additional arguments passed to this script are forwarded as-is to the
# sync-embeddings-to-vector-db command. Example:
#   ./sync-embeddings-to-vector-db.sh --dry-run --force-rewrite

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
  "$@"

echo "Done"
