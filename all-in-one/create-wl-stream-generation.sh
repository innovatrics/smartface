#!/bin/bash

getvalue() {
    local key="$1"
    local value=$(grep -E ^${key}= .env | cut -d '=' -f2- | cut -d$'\r' -f1)
    echo "$value"
}

docker run --rm --name admin-stream-gen --network sf-network $(getvalue REGISTRY)sf-admin:$(getvalue SF_VERSION) \
    populate-wl-update-log-stream \
    -c "$(getvalue ConnectionStrings__CoreDbContext)" \
    -dbe $(getvalue Database__DbEngine) \
    --rmq-host "$(getvalue RabbitMQ__Hostname)" --rmq-user "$(getvalue RabbitMQ__Username)" --rmq-pass "$(getvalue RabbitMQ__Password)" \
    --rmq-virtual-host "$(getvalue RabbitMQ__VirtualHost)" --rmq-port "$(getvalue RabbitMQ__Port)" --rmq-use-ssl "$(getvalue RabbitMQ__UseSsl)" --rmq-streams-port "$(getvalue RabbitMQ__StreamsPort)"
