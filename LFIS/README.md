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

1. To start migration of face templates, execute
```
./migrate-faces.sh
```

This will stop the current compose services, spawn the required face detector and extractor services, and run the migration CLI command. After this, you should see output regarding the success rate of migration and also a list of watchlist members for which template migration was not possible. You should store this output to handle those members' faces manually by requesting reenrollment of their faces.

> **Note (1):** You can override the default template model version (`53`) by setting `FACE_MODEL_VERSION` env variable before running the script. Possible values are:
 - `52` (`fast`)
 - `53` (`balanced`)
 - `54` (`accurate`)
 - `55` (`accurate_server`)

> **Note (2):** It is possible that there were some transient errors while running this script (e.g. some RPC calls may timeout). In that case, it is safe to run this command again.

2. To finalize migration, execute
```
./finalize-non-migrated-faces.sh
```
This will force the remaining faces that were not possible to migrate to be set to error state and thus be skipped by our matchers at startup.

3. You should be able to run compose services successfully again (e.g. by executing)
```
docker compose up -d
```

### Syncing embeddings to the vector database

SmartFace can optionally store face and palm embeddings in a vector database (Milvus) for matching. The SQL database remains the source of truth; the vector database is populated from it. A Milvus service (together with its `milvus-etcd` dependency) is bundled in `sf_dependencies/docker-compose.yml` and starts together with the other dependencies.

1. To sync the embeddings currently stored in the SQL database into the vector database, execute
```
./sync-embeddings-to-vector-db.sh
```

This starts the bundled Milvus service (if it is not already running) and runs the `sync-embeddings-to-vector-db` CLI command, which reads the face and palm embeddings from the SQL database and indexes them into the vector database. It does not stop the running SmartFace services.

> **Note:** The vector database is partitioned into one database per tenant. The command reports how many embeddings were indexed per tenant.

2. The script can be customized with the following environment variables:
 - `VECTOR_DB_ENDPOINT` — the vector database endpoint to populate. Default: `http://milvus:19530` (the bundled Milvus service).
 - `BATCH_SIZE` — number of watchlist members loaded per SQL database paging batch. Default: `1000`.
 - `DRY_RUN` — set to `true` to verify the SQL and vector database connections and report how many embeddings would be indexed, without writing anything.
 - `FORCE_REWRITE` — set to `true` to drop and recreate the embedding collections before indexing. This is required when the collections already exist. It is destructive: the collections are dropped before indexing begins, so an interrupted run can leave them empty. Since the SQL database remains the source of truth, the collections can always be rebuilt by re-running the script.

For example, to preview a sync without writing anything:
```
DRY_RUN=true ./sync-embeddings-to-vector-db.sh
```

And to re-sync into already populated collections:
```
FORCE_REWRITE=true ./sync-embeddings-to-vector-db.sh
```
