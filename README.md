SmartFace is a high-performance, scalable, face recognition server platform able to process multiple real-time video streams in parallel. Leveraging Innovatrics’ industry-leading algorithm, SmartFace allows system integrators to easily incorporate face recognition into their solutions. https://www.innovatrics.com/face-recognition-solutions/

# Integrating with SmartFace
Any system can integrate with SmartFace by using API (OpenAPI/OData) and pub-sub notifications (ZeroMQ).

For more information see:
* [OpenAPI](https://innovatrics.github.io/smartface/?url=https://raw.githubusercontent.com/innovatrics/smartface/4.11/api/swagger.json) - Swagger UI
* [sfcli](sfcli) - You can use command line based user interface to interact with the Smart Face API and receive notifications.
* [docker](sf-docker) - SmartFace can be easily deployed using docker containers. 

# Getting Started
1. Deploy SmartFace (ask for demo/installation https://www.innovatrics.com/face-recognition-solutions/)

2. Start video face recognition using following CLI command (or use GUI)
```
$ sfcli camera add -s:c:\temp\wafs.mp4 -e:true
```

3. Recognized faces - receive notifications using following CLI command (or use GUI)
```
$ sfcli --zero-mq-host localhost notifications -t:faces.insert
```
