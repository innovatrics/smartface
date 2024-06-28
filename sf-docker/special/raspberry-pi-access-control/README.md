# SmartFace Raspberry Pi

## Deployment
1. Install `Docker` and `docker compose` on the host machine.
2. Login to container registry `docker login registry.gitlab.com -u <username> -p <password>`. The credentials are available in our [Customer Portal](https://customerportal.innovatrics.com/).
3. Identify hardware id (hwid) for your machine with command `docker run --privileged registry.gitlab.com/innovatrics/smartface/license-manager:3.2.7`. This process work for native linux Ubuntu running on the Raspberry Pi system.
4. Obtain license for your hwid from our Customer Portal https://customerportal.innovatrics.com/
5. Copy the license file `iengine.lic` to the root of this directory.
6. Run `run.sh` script. The run scripts contain comments which should clarify the steps needed to start everything