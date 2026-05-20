# SmartFace with KeyCloak

SmartFace + KeyCloak behind an HTTPS NGINX reverse proxy.

## Deploy

```bash
./run.sh
```

NGINX routes: `/` → Station, `/api/` → REST, `/graphql` → GraphQL, `/auth/` → KeyCloak.
Port 80 redirects to 443.

## Retargeting to a new host

Everything is hardcoded to `presales-demo-1u.ba.innovatrics.net`. To redeploy
elsewhere, search/replace that string across the repo:

- `keycloak-server/realm-export.json` (rootUrl, adminUrl, redirectUris)
- `keycloak-server/docker-compose.yml` (KEYCLOAK_FRONTEND_URL)
- `sf-server/.env.sfstation` (KEYCLOAK_DOMAIN, KEYCLOAK_JWKS_URI, KEYCLOAK_ADMIN_URL)
- `nginx/conf.d/default.conf` (server_name + ssl_certificate paths)

Then drop your TLS cert + key into `certs/` and update the two `ssl_certificate*`
filenames in `nginx/conf.d/default.conf` to match.

## Notes

- `realm-export.json` is imported only on KeyCloak's first launch. After a host
  change against an existing KeyCloak DB, wipe its data volume or update client
  URLs via the KeyCloak admin UI.
- Ports `8080` (KeyCloak) and `8000` (Station) stay published for debugging.
