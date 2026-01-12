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

FACE_MODEL_VERSION=${FACE_MODEL_VERSION:-53}

echo "Calling set-state-error-non-migrated-faces command to mark faces that cannot be migrated to error state"

docker run --rm --name sf_admin \
    --volume "$(pwd)/iengine.lic:/etc/innovatrics/iengine.lic" \
    --network sf-network \
    "${SF_ADMIN_IMAGE}" \
    set-state-error-non-migrated-faces \
    -c "$(getvalue ConnectionStrings__CoreDbContext)" \
    -dbe "$(getvalue Database__DbEngine)" \
    --face-model-version "${FACE_MODEL_VERSION}"

echo "Done"
echo "You can start the services again with docker compose up -d"
