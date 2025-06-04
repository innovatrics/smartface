# SmartFace with KeyCloak

KeyCloak servers as identity provider on-premise. In this sample, KeyCloak is deployed on the same machine. In production deployments KeyCloak is deployed on standalone machine with extra security.

## Deploy

1. Before you launch your docker containers, replace `$YOUR-SERVER-IP$` with the IP address of server with KeyCloak. When deployed publicly on the internet, replace with FQDN (keycloak.mySuperDomain.com)
2. Enter `keycloak-server` and run `docker-compose up -d`
3. Enter `sf-server` and run `run.sh`

KeyCloak contains pre-defined configuration in `keycloak-server\realm-export.json`. Feel free to replace with your own KeyCloak configuration.
