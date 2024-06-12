# If yq is not present on the machine, you can run this script using docker image mikefarah/yq by running following command from sf-docker directory
# docker run -it --rm -v ${PWD}:/workdir --entrypoint /bin/sh mikefarah/yq ./special/sf-2-sf-synchronization/regenerate.sh

THISDIR=$(dirname "$0")
DOCKERDIR=$(dirname $(dirname $THISDIR))

cp $DOCKERDIR/all-in-one/.env $THISDIR/leader/.env
cp $DOCKERDIR/all-in-one/.env.sfac $THISDIR/leader/.env.sfac
cp $DOCKERDIR/all-in-one/.env.sfstation $THISDIR/leader/.env.sfstation
cp $DOCKERDIR/all-in-one/run.sh $THISDIR/leader/run.sh
cat $DOCKERDIR/all-in-one/docker-compose.yml \
    | yq -P '
        with (.services.db-synchronization-leader;
            .image = "${REGISTRY}sf-db-synchronization-leader:${SF_VERSION}"
            | .container_name = "SFDbSynchronizationLeader"
            | .ports = [ "8100:${Hosting__Port}" ]
            | .restart = "unless-stopped"
            | .environment = [ 
                "RabbitMQ__Hostname",
                "RabbitMQ__Username",
                "RabbitMQ__Password",
                "RabbitMQ__Port",
                "RabbitMQ__StreamsPort",
                "RabbitMQ__VirtualHost",
                "RabbitMQ__UseSsl",
                "Hosting__Host",
                "Hosting__Port",
                "AppSettings__Log_RollingFile_Enabled=false",
                "AppSettings__USE_JAEGER_APP_SETTINGS",
                "JAEGER_AGENT_HOST",
                "S3Bucket__Endpoint",
                "S3Bucket__BucketName",
                "S3Bucket__AccessKey",
                "S3Bucket__SecretKey",
                "Authentication__UseAuthentication",
                "Authentication__IgnoreHttpsIssuerCheck",
                "Authentication__Authority",
                "Authentication__Audience" ]
            | .volumes = [ "./iengine.lic:/etc/innovatrics/iengine.lic" ]
        )
    ' > $THISDIR/leader/docker-compose.yml

cp $DOCKERDIR/all-in-one/.env $THISDIR/follower/.env
cp $DOCKERDIR/all-in-one/.env.sfac $THISDIR/follower/.env.sfac
cp $DOCKERDIR/all-in-one/.env.sfstation $THISDIR/follower/.env.sfstation
cp $DOCKERDIR/all-in-one/run.sh $THISDIR/follower/run.sh
cat $DOCKERDIR/all-in-one/docker-compose.yml \
    | yq -P '
        with (.services.api; .environment |= . + "FeatureManagement__ReadOnlyWatchlists=true" ) |
        with (.services.sf-station;
            .environment = []
            | .environment |= . + "FeatureManagement__ReadOnlyWatchlists=true" ) |
        with (.services.db-synchronization-follower;
            .image = "${REGISTRY}sf-db-synchronization-follower:${SF_VERSION}"
            | .container_name = "SFDbSynchronizationFollower"
            | .ports = [ "8100:${Hosting__Port}" ]
            | .restart = "unless-stopped"
            | .environment = [ 
                "RabbitMQ__Hostname",
                "RabbitMQ__Username",
                "RabbitMQ__Password",
                "RabbitMQ__Port",
                "RabbitMQ__StreamsPort",
                "RabbitMQ__VirtualHost",
                "RabbitMQ__UseSsl",
                "ConnectionStrings__CoreDbContext",
                "Database__DbEngine",
                "AppSettings__Log_RollingFile_Enabled=false",
                "AppSettings__USE_JAEGER_APP_SETTINGS",
                "JAEGER_AGENT_HOST",
                "S3Bucket__Endpoint",
                "S3Bucket__BucketName",
                "S3Bucket__AccessKey",
                "S3Bucket__SecretKey",
                "Leader__Address",
                "ClientAuthentication__UseAuthentication",
                "ClientAuthentication__TokenEndpoint",
                "ClientAuthentication__ClientId",
                "ClientAuthentication__ClientSecret",
                "ClientAuthentication__Audience" ]
            | .volumes = [ "./iengine.lic:/etc/innovatrics/iengine.lic" ]
        )
    ' > $THISDIR/follower/docker-compose.yml