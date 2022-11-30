# SmartFace Access Control
Primary role of the SmartFace Access Control is to provide controlled access to some area using an integration with physical security elements like doors, turnistilles or gateways.
For example, when an identified person approaches entrance to the building, the reception turnstille opens automatically.

## Design
We have enabled services only required for the Access Control use case. Other functionality may be affected or disabled.
4 Camera services are available (can be extended), all APIs, detector, extractor and matching service.
SmartFace Station GUI is included as well.

## Quick Start
1. Install `Docker` and `docker compose` on the host machine.
2. Login to container registry `docker login registry.gitlab.com -u <username> -p <password>`. The credentials are available in our [CRM portal](https://crm.innovatrics.com/).
3. Identify hardware id (hwid) for your machine with command `docker run registry.gitlab.com/innovatrics/smartface/license-manager:3.2.7`. This process work for native linux, for `WSL2` eg. linux containers on Windows you need special license for which you need to contact our sales.
4. Obtain license for your hwid from our CRM https://crm.innovatrics.com/client/products
5. Copy the license file `iengine.lic` to the root of this directory.
6. Run `run.sh` script. The run scripts contain comments which should clarify the steps needed to start everything