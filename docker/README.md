To run SmartFace in docker simply copy the license file `iengine.lic` to the root of this directory and then run one of the run scripts:
- `run.sh` - to run full SF platform on x64
- `run-jetson.sh` - to run SF platform on Nvidia Jetson devices
- `run-cloud-matcher.sh` - to run cloud matcher

The provided images do not contain a SmartFace license, so you need to provide your own license. Since these are docker images, the license needs to be universal (not be bound to specific HWID).

The run scripts contain comments which should clarify the steps needed to start everything.

The provided docker-compose files are used to demonstrate configuration steps needed to wire everything up, but the images can be used with any other orchestration engine. Also note that not all services are needed for every use case, e.g the `video-*` services are used for offline video processing, so if offline video processing is not to be used they can be disabled (commented-out).
