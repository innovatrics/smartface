# SmartFace Lightweight Facial Identification Service

## Deployment
1. Install `Docker` and `docker compose` on the host machine.
2. Login to container registry `docker login registry.gitlab.com -u <username> -p <password>`. The credentials are available in our [Customer Portal](https://customerportal.innovatrics.com/).
3. Identify hardware id (hwid) for your machine with command `docker run registry.gitlab.com/innovatrics/smartface/license-manager:3.2.7`. This process work for native linux, for `WSL2` eg. linux containers on Windows you need special license for which you need to contact our sales.
4. Obtain license for your hwid from our Customer Portal https://customerportal.innovatrics.com/
5. Copy the license file `iengine.lic` to the root of this directory.
6. Run `run.sh` script. The run scripts contain comments which should clarify the steps needed to start everything

### Palm templates migration

1. To start migration of palm templates, execute
```
./migrate-palms.sh
```

This will stop the current compose services, spawn the required palm detector and extractor services, and run the migration CLI command. After this, you should see output regarding the success rate of migration and also a list of watchlist members for which template migration was not possible. You should store this output to handle those members' palms manually by requesting reenrollment of their palms.
> **Note:** It is possible that there were some transient errors while running this script (e.g. some RPC calls may timeout). In that case, it is safe to run this command again.

2. To finalize migration, execute
```
./finalize-non-migrated-palms.sh
```
This will force the remaining palms that were not possible to migrate to be set to error state and thus be skipped by our matchers at startup.

3. You should be able to run compose services successfully again (e.g. by executing)
```
docker compose up -d
```

### Face templates migration
> **Note:** TBD