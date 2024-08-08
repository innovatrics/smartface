docker kill $(docker ps -a -q)

docker rm $(docker ps -a -q)

docker volume prune --force
docker network prune --force

git fetch -p; git pull;
