#!/bin/sh

source .env

docker run --rm --name admin-stream-gen --network sf-network ${REGISTRY}sf-admin:${VERSION} \
    populate-wl-update-log-stream \
    -c "${ConnectionStrings__CoreDbContext}" \
    -dbe ${Database__DbEngine} \
    --rmq-host ${RabbitMQ__Hostname} --rmq-user ${RabbitMQ__Username} --rmq-pass ${RabbitMQ__Password} \
    --rmq-virtual-host ${RabbitMQ__VirtualHost} --rmq-port ${RabbitMQ__Port} --rmq-use-ssl ${RabbitMQ__UseSsl} --rmq-streams-port ${RabbitMQ__StreamsPort}