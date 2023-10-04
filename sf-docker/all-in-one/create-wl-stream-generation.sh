# TODO consider not using this as a separate scirpt, but maybe add it into the run.sh script? maybe also only optional? Also modify README for relevant deployments if the generation creation is an optional step
# TODO consider also adding the command for windows variant into the github? or is documentation enough? we should also have an example command with values in documentation

# TODO get these values from .env file - see run.sh script
REGISTRY="registry.gitlab.com/innovatrics/smartface/"
VERSION="v5_4.22.0.4714-dev"
DB_ENGINE="PgSql"
RMQ_HOST="rmq"
RMQ_PASS="guest"
RMQ_USER="guest"
RMQ_VHOST="/"
RMQ_PORT="5672"
RMQ_SSL="false"
RMQ_STREAMS_PORT="5552"

# TODO connection string
docker run --rm --name admin-stream-gen --network sf-network ${REGISTRY}sf-admin:${VERSION} populate-wl-update-log-stream -c "Server=pgsql;Database=smartface;Username=postgres;Password=Test1234;Trust Server Certificate=true;" -dbe $DB_ENGINE --rmq-host ${RMQ_HOST} --rmq-user ${RMQ_USER} --rmq-pass ${RMQ_PASS} --rmq-virtual-host ${RMQ_VHOST} --rmq-port ${RMQ_PORT} --rmq-use-ssl ${RMQ_SSL} --rmq-streams-port ${RMQ_STREAMS_PORT}
