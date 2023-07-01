<img src="https://github.com/jpcarpanezi/houston/blob/dev/src/Web/WebAngular/src/assets/houston.png" width=100 alt="Houston logo">

# Houston &middot; [![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/jpcarpanezi/houston/blob/dev/LICENSE) [![Deploy API](https://github.com/jpcarpanezi/houston/actions/workflows/deploy-api.yml/badge.svg)](https://github.com/jpcarpanezi/houston/actions/workflows/deploy-api.yml) [![Deploy Worker](https://github.com/jpcarpanezi/houston/actions/workflows/deploy-worker.yml/badge.svg)](https://github.com/jpcarpanezi/houston/actions/workflows/deploy-worker.yml) [![Deploy Front](https://github.com/jpcarpanezi/houston/actions/workflows/deploy-front.yml/badge.svg)](https://github.com/jpcarpanezi/houston/actions/workflows/deploy-front.yml)
Houston is an open-source project aimed at providing a user-friendly and intuitive platform for continuous integration (CI) deployment using low-code techniques. With Houston, users can easily create and manage CI pipelines through a simple and intuitive interface.

## Getting Started with Docker Compose 🚀
To get started with Houston using Docker Compose, follow the steps below:

1. Generate an authentication private key by running the following command in your terminal:
```
ssh-keygen -t rsa -b 4096 -m PEM -N "" -f /data/houston/PrivateKey.pem
```

2. Create a `docker-compose.yaml` file with the provided [docker-compose.yaml](https://github.com/jpcarpanezi/houston/blob/dev/docker-compose.yaml) template:
   
3. In the docker-compose.yaml file, replace `<host-origin>`, `<postgres-password>`, `<redis-password>`, and `<rabbit-password>` with your desired passwords.
   
4. Run the following command to start the Houston containers using Docker Compose:
```
docker-compose up -d --build
```

5. Wait for the containers to start up. Once the process is complete, you can access the Houston web interface through your preferred web browser.

**Note:** Make sure you have Docker and Docker Compose installed on your machine before proceeding.

## Contributing ✨
Thank you for your interest in contributing to Houston! Currently, we are not accepting external contributions to the project. However, we appreciate your enthusiasm and support.

If you have any suggestions, bug reports, or feature requests, please feel free to open an [issue](https://github.com/jpcarpanezi/houston/issues). Your feedback is valuable to us, and it helps us improve the platform.

We will update the community if there are any changes regarding contributions in the future. We appreciate your understanding.

## Support 🤝
If you encounter any issues or have any questions or suggestions, please feel free to open an [issue](https://github.com/jpcarpanezi/houston/issues). Our team will be happy to assist you.

## License 📄
Houston is licensed under the MIT License, which means you are free to use, modify, and distribute the software. Please see the [LICENSE](https://github.com/jpcarpanezi/houston/blob/dev/LICENSE) file for more details.
