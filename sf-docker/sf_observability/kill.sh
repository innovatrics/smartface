docker kill $(docker ps -a -q)

docker rm $(docker ps -a -q)

docker volume prune --force --all

git fetch -p; git pull;
