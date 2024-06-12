# SmartFace on Docker

SmartFace docker images provide an easy way of deploying and scaling SmartFace with all the benefits of containerization. SmartFace platform is distributed as Linux Docker images for both 86x64 and ARM platforms.

# Deployment

Before deploying SF, you will need:

- Docker
- docker-compose
- Login to container registry `docker login registry.gitlab.com -u <username> -p <password>`. The credentials are available in our [Customer Portal](https://customerportal.innovatrics.com/).

## License

In order to run SmartFace, you need a valid license.

- Identify hardware id (hwid) for your machine with the command `docker run registry.gitlab.com/innovatrics/smartface/license-manager:3.2.7`. This process work for native Linux, for `WSL2` eg. linux containers on Windows you need a special license for which you need to contact our sales.
- Obtain a license for your hwid from our Customer Portal https://customerportal.innovatrics.com/
- Copy the license file `iengine.lic` to the directory where `docker-compose.yml` is located

## Samples

To get up and running as fast as possible, these samples are available:
- [`all-in-one`](./all-in-one/) - a All-in-One setup known from previous version. Contains all available services.
- [`LFIS`](./LFIS/) - Lightweight Facial Identification Service deployment sample. Contains a subset of services.

# GPU acceleration

Some services can benefit from GPU acceleration, which can be enabled in docker compose file, but also some prerequisites needs to be met on host machine.

Please note that GPU acceleration is supported only on NVIDIA GPU.

To use GPU acceleration, you will need following on the docker host machine:

- Nvidia GPU compatible with Cuda 11.6
- Nvidia driver of version >=510.47.03
- Nvidia container toolkit https://docs.nvidia.com/datacenter/cloud-native/container-toolkit/install-guide.html#docker

To use GPU for hw decoding and face detection for cameras uncomment `runtime: nvidia` and `GstPipelineTemplate` in `docker-compose.yml` for camera services `sf-cam-*`.
When using the nvidia docker runtime SmartFace camera processes need gstreamer pipelines for camera sources.

Other services that could use GPU needs also uncomment environment variable `Gpu__GpuEnabled=true`. This is necessary for extractor, detector, pedestrian-detector and liveness service.

For using specific neural networks runtime it is possible to uncomment environment variable `Gpu__GpuNeuralRuntime` which can have values `Default`, `Cuda` or `Tensor`. The GPU needs to support these neural runtimes. When using `Tensor` you can uncomment mapping `"/var/tmp/innovatrics/tensor-rt:/var/tmp/innovatrics/tensor-rt"` to retain TensorRT cache files in the host when a container is recreated. This can be helpful as generating cache files is a longer operation which needs to be performed before the first run of the neural network. Setting neural network runtime is possible for a camera, extractor, detector, pedestrian-detector and liveness services.

To use GPU on an ARM device, such as NVIDIA Jetson, we suggest using the SmartFace Embedded Stream Processor together with the SmartFace Platform.

## Production use

The provided samples are used to demonstrate the configuration steps needed to wire everything up and are not fit for production use. The images can be used with any other orchestration engine.
