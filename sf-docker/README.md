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
Some services can benefit from GPU acceleration, which can be enabled in docker compose file, but also some prerequisites needs to  be met on host machine.

Please note that GPU acceleration is supported only on NVIDIA GPU.

To use GPU acceleration, you will need following on the docker host machine:
- Nvidia GPU compatible with Cuda 10.1
- Nvidia driver of version >= 418.39
- Nvidia container toolkit https://docs.nvidia.com/datacenter/cloud-native/container-toolkit/install-guide.html#docker

To use GPU for hw decoding and face detection for cameras uncomment `runtime: nvidia` and `GstPipelineTemplate` in `docker-compose.yml` for camera services `sf-cam-*`. 
When using the nvidia docker runtime SmartFace camera processes need gstreamer pipelines for camera sources.

Other services which could use GPU needs also uncomment environment variable `Gpu__GpuEnabled=true`. This is necessary for extractor, detector, pedestrian-detector and liveness service.

# Production use
The provided docker-compose files are used to demonstrate configuration steps needed to wire everything up and are not fit for production use. The images can be used with any other orchestration engine. Also note that not all services are needed for every use case, e.g the `video-*` services are used for offline video processing, so if offline video processing is not to be used they can be disabled (commented-out).
