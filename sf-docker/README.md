# About
SmartFace docker images provide an easy way of deploying and scaling SmartFace with all the benefits of containerization. SmartFace platform is distributed as a number of linux docker images, some of which are specific for [Nvidia Jetson](https://developer.nvidia.com/embedded/jetson-developer-kits) platform.

# Deployment
Before deploying SF, you will need:
- Docker
- docker-compose
- Login to container registry `docker login registry.gitlab.com -u <username> -p <password>`. The credentials are available in our [CRM portal](https://crm.innovatrics.com/).
- Copy the license file `iengine.lic` to the root of this directory. Since these are docker images, the license needs to be universal (not be bound to specific HWID).

To get up and running as fast as possible, multiple run scripts are available for different platforms.
The run scripts contain comments which should clarify the steps needed to start everything:
- `run.sh` - to run full SF platform on x64
- `run-jetson.sh` - to run SF platform on Nvidia Jetson devices. Note that only PgSQL database is available on arm architecture, so modify `.env` file accordingly
- `run-cloud-matcher.sh` - to run cloud matcher

# GPU acceleration
Some services could benefit from GPU acceleration, which could be enabled in docker compose file. 

To use GPU for hw decoding and face detection uncomment `runtime: nvidia` in `docker-compose.yml` for camera services `sf-cam-*`. 

Other services which could use GPU needs also uncomment environment variable `Gpu__GpuEnabled=true`. This is necessary for extractor, detector, pedestrian-detector, liveness.

Please note that GPU acceleration is supported only NVIDIA GPU.

# Production use
The provided docker-compose files are used to demonstrate configuration steps needed to wire everything up and are not fit for production use. The images can be used with any other orchestration engine. Also note that not all services are needed for every use case, e.g the `video-*` services are used for offline video processing, so if offline video processing is not to be used they can be disabled (commented-out).
