# SmartFace to SmartFace Watchlist synchronization

In this setup, you can have one *Leader* SmartFace instance and multiple "Follower" SmartFace instances. 
Changes in Watchlists and Watchlist members are automatically synchronized from *Leader* to all 
*Followers*. This setup is especially useful when the SmartFace instances are geographically separated 
and are required to work independently even in case of connection errors.

The *Leader* SmartFace instance is normal instance with SFDbSyncLeader service and exposed TCP port (in 
this example, port 8100).

Each *Follower* SmartFace instance is also a normal instance, but with API and SmartFace Station services
in *read only watchlist mode* and with SFDbSyncFollower service configured to the SFDbSyncLeader endpoint.

In production environment, usage of TLS and authentication is greatly encouraged.

You can find out more about this setup in [online documentation](https://developers.innovatrics.com/smartface/docs/manuals/smartface-platform/watchlists/#watchlist-synchronization-between-smartface-instances).