# If yq is not present on the machine, you can run this script using docker image mikefarah/yq by running following command from sf-docker directory
# docker run -it --rm -v $PWD:/workdir --entrypoint /bin/sh mikefarah/yq ./special/sf-2-sf-synchronization/regenerate.sh

BASEDIR=$(dirname "$0")

cp $BASEDIR/../../all-in-one/.env $BASEDIR/leader/.env
cp $BASEDIR/../../all-in-one/.env.sfac $BASEDIR/leader/.env.sfac
cp $BASEDIR/../../all-in-one/.env.sfstation $BASEDIR/leader/.env.sfstation
cp $BASEDIR/../../all-in-one/run.sh $BASEDIR/leader/run.sh
cat $BASEDIR/../../all-in-one/docker-compose.yml \
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
    ' > $BASEDIR/leader/docker-compose.yml

cp $BASEDIR/../../all-in-one/.env $BASEDIR/follower/.env
cp $BASEDIR/../../all-in-one/.env.sfac $BASEDIR/follower/.env.sfac
cp $BASEDIR/../../all-in-one/.env.sfstation $BASEDIR/follower/.env.sfstation
cp $BASEDIR/../../all-in-one/run.sh $BASEDIR/follower/run.sh
cat $BASEDIR/../../all-in-one/docker-compose.yml \
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
    ' > $BASEDIR/follower/docker-compose.yml