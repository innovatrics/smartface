# About
SmartFace docker images provide an easy way of deploying and scaling SmartFace with all the benefits of containerization. SmartFace platform is distributed as a number of linux docker images, some of which are specific for [Nvidia Jetson](https://developer.nvidia.com/embedded/jetson-developer-kits) platform.

# Deployment
Before deploying SF, you will need:
- Docker
- docker-compose
- Login to container registry `docker login registry.gitlab.com -u <username> -p <password>`. The credentials are available in our CRM portal.
- Copy the license file `iengine.lic` to the root of this directory. Since these are docker images, the license needs to be universal (not be bound to specific HWID).

To get up and running as fast as possible, multiple run scripts are available for different platforms.
The run scripts contain comments which should clarify the steps needed to start everything:
- `run.sh` - to run full SF platform on x64
- `run-jetson.sh` - to run SF platform on Nvidia Jetson devices
- `run-cloud-matcher.sh` - to run cloud matcher

# Production use
The provided docker-compose files are used to demonstrate configuration steps needed to wire everything up and are not fit for production use. The images can be used with any other orchestration engine. Also note that not all services are needed for every use case, e.g the `video-*` services are used for offline video processing, so if offline video processing is not to be used they can be disabled (commented-out).
