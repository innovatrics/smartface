# If yq is not present on the machine, you can run this script using docker image mikefarah/yq by running following command from sf-docker directory
# docker run -it --rm -v ${PWD}:/workdir --entrypoint /bin/sh mikefarah/yq ./special/sf-2-sf-synchronization/regenerate.sh

THISFILE=$(readlink -f "$0")
THISDIR=$(dirname "$THISFILE")
DOCKERDIR=$(dirname $(dirname "$THISDIR"))

cp $DOCKERDIR/all-in-one/.env $THISDIR/leader/.env
cp $DOCKERDIR/all-in-one/.env.sfac $THISDIR/leader/.env.sfac
cp $DOCKERDIR/all-in-one/.env.sfstation $THISDIR/leader/.env.sfstation
cp $DOCKERDIR/all-in-one/run.sh $THISDIR/leader/run.sh
cp $DOCKERDIR/all-in-one/docker-compose-common.yml $THISDIR/leader/docker-compose-common.yml
cp $DOCKERDIR/all-in-one/docker-compose.yml $THISDIR/leader/docker-compose.yml

cp $DOCKERDIR/all-in-one/.env $THISDIR/follower/.env
cp $DOCKERDIR/all-in-one/.env.sfac $THISDIR/follower/.env.sfac
cp $DOCKERDIR/all-in-one/.env.sfstation $THISDIR/follower/.env.sfstation
cp $DOCKERDIR/all-in-one/run.sh $THISDIR/follower/run.sh
cp $DOCKERDIR/all-in-one/docker-compose-common.yml $THISDIR/follower/docker-compose-common.yml
cat $DOCKERDIR/all-in-one/docker-compose.yml \
    | yq -P '
        with (.services.api.environment; .FeatureManagement__ReadOnlyWatchlists = "true" )
        | with (.services.sf-station.environment; .FeatureManagement__ReadOnlyWatchlists = "true" )
        | del .services.db-synchronization-leader
        | with (.services.db-synchronization-follower;
            with (.extends;
                .service = "common-rabbitmq-db-s3"
                | .file = "docker-compose-common.yml" )
            | .image = "${REGISTRY}sf-db-synchronization-follower:${SF_VERSION}"
            | with (.environment;
                .Leader__Address = "" 
                | .Leader__Address tag = "!!null"
                | .ClientAuthentication__UseAuthentication = "" 
                | .ClientAuthentication__UseAuthentication tag = "!!null"
                | .ClientAuthentication__TokenEndpoint = "" 
                | .ClientAuthentication__TokenEndpoint tag = "!!null"
                | .ClientAuthentication__ClientId = "" 
                | .ClientAuthentication__ClientId tag = "!!null"
                | .ClientAuthentication__ClientSecret = "" 
                | .ClientAuthentication__ClientSecret tag = "!!null"
                | .ClientAuthentication__Audience = "" 
                | .ClientAuthentication__Audience tag = "!!null"
                )
        )
    ' > $THISDIR/follower/docker-compose.yml
