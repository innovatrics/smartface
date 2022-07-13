# SmartFace on Docker
SmartFace docker images provide an easy way of deploying and scaling SmartFace with all the benefits of containerization. SmartFace platform is distributed as a number of linux docker images, some of which are specific for [Nvidia Jetson](https://developer.nvidia.com/embedded/jetson-developer-kits) platform.

## Before we start
Before deploying SmartFace, you will need:
- Docker
- docker-compose
- Login to container registry `docker login registry.gitlab.com -u <username> -p <password>`. The credentials are available in our [CRM portal](https://crm.innovatrics.com/).

## License
In order to run SmartFace, you need a valid license.

- Identify hardware id (hwid) for your machine with command `docker run registry.gitlab.com/innovatrics/smartface/license-manager:3.2.7`. For nvidia jetson device use command `docker run --privileged registry.gitlab.com/innovatrics/smartface/license-manager:3.2.7`. This process work for native linux, for `WSL2` eg. linux containers on Windows you need special license for which you need to contact our sales.
- Obtain license for your hwid from our CRM https://crm.innovatrics.com/client/products
- Copy the license file `iengine.lic` to the directory where `docker-compose.yml` is located

## Samples
To get up and running as fast as possible, multiple scenarios are available:
- [`hello-world`](./hello-world/) - a legendary *Hello World!* is a must have
- [`cloud-matcher`](./hello-world/) - Cloud Matcher deployment sample
- [`nvidia-jetson`](./hello-world/) - launch demo deployment on [Nvidia Jetson](https://developer.nvidia.com/embedded/jetson-developer-kits) platform
- [`access-controller`](./hello-world/) - preconfigured for the Access Controll use case


## GPU acceleration
Some services can benefit from GPU acceleration, which can be enabled in docker compose file, but also some prerequisites needs to  be met on host machine.

Please note that GPU acceleration is supported only on NVIDIA GPU.

To use GPU acceleration, you will need following on the docker host machine:
- Nvidia GPU compatible with Cuda 11.1
- Nvidia driver of version >= 450.80.02
- Nvidia container toolkit https://docs.nvidia.com/datacenter/cloud-native/container-toolkit/install-guide.html#docker

To use GPU for hw decoding and face detection for cameras uncomment `runtime: nvidia` and `GstPipelineTemplate` in `docker-compose.yml` for camera services `sf-cam-*`. 
When using the nvidia docker runtime SmartFace camera processes need gstreamer pipelines for camera sources.

Other services which could use GPU needs also uncomment environment variable `Gpu__GpuEnabled=true`. This is necessary for extractor, detector, pedestrian-detector and liveness service.

For using specific neural networks runtime it is possible to uncomment environment variable `Gpu__GpuNeuralRuntime` which can have values `Default`, `Cuda` or `Tensor`. The GPU needs to support these neural runtimes. When using `Tensor` you can uncomment mapping `"/var/tmp/innovatrics/tensor-rt:/var/tmp/innovatrics/tensor-rt"` to retain TensorRT cache files in the host when container is recreated. This can be helpful as generating cache files is longer operation which needs to be performed before the first run of neural network. Setting neural network runtime is possible for camera, extractor, detector, pedestrian-detector and liveness services.

## Production use
The provided samples are used to demonstrate configuration steps needed to wire everything up and are not fit for production use. The images can be used with any other orchestration engine.