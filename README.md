# The Featured Bugz - User Service
Welcome to the README for the User Service project developed by The Featured Bugz team. This project consists of two main projects: the API testing project and the userServiceAPI project and is made in **`.Net 7.0`**. The userServiceAPI provides functionality for managing user data and interacts with a MongoDB database.

## Table of Contents

- Overview
- API Endpoints
    - Post User
    - Get User
    - Version
- Getting Started
    - Prerequisites
    - Installation
    - Configuration
- Running with Docker
- Contributing
- License

## Overview

The User Service project is designed to provide an API for managing user data. It offers three main API endpoints: **`postUser`**, **`getUser`**, and **`version`**. The postUser endpoint allows you to create a new user and store the data in a **`MongoDB database`**. The getUser endpoint retrieves user information based on the provided username and password. The version endpoint returns the project's version written in the.

## API Endpoints
### Post User

```bash
POST /api/postUser
```

This endpoint is used to create a new user and store the data in a MongoDB database.

**Request Body**
The request should include the following propeties and this is a example:

```json
{
    "userName": "John",
    "userPassword": "Password123",
    "userEmail": "John@gmail.com",
    "userAddress": "Johnny way 23"
}
```
**Response**
If the user is successfully created and stored in the database, the API will respond with a success status code **`(200 OK)`**. In case of any error, an appropriate error status code will be returned.

### Get User

```bash
GET /api/getUser?UserName={username}&UserPassword={password}
```
This endpoint allows you to retrieve user information based on the provided username and password.

**Parameters**

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `UserName` | `string` | The username of the user to retrieve |
| `UserPassword` | `string` | The password of the user to retrieve |

**Response**

If the user with the specified username and password is found in the database, the API will respond with the user's data. If the user is not found, the API will return a status code **`404 Not Found`**.

### Version
```bash
GET /api/version
```
This endpoint allows you to retrieve the version information of the userServiceAPI project.

**Response**

The API will respond with the version information of the project.

## Getting Started

To get started with the User Service project, follow the instructions below.

**Prerequisites**

Before running the project, make sure you have the following installed:

- Bash
- MongoDB
- Visual Studio Code or Visual Studio
- Can use Docker desktop
- Can use Postman for api calls.
- .Net 7.0

**Installation**
1. Clone the repository to your local machine.

**Configuration**
Before starting the API, you may need to configure some settings. Open the **`startup.sh`** file and modify it according to your requirements.

## **Running localhost API**
1. Navigate to userServiceAPI folder.
2. Start the project with the following command:
```bash
. ./startup.sh
```
3. Make a api call 

## Running with Docker 

The User Service API can also be set up using Docker. A Dockerfile is provided to simplify the setup process. Make sure you have Docker desktop installed on your system, and then follow these steps:

1. Build the Docker image using the following command:
```bash
docker build -t user-service-image Dockerfile .
```
2. One the image is built, you can run the API container:
```bash
docker run -d -p 5081:80 --name user-service user-service-image
```
The API will be accessible at **`http://localhost:5081`**.

## Contributing

Contributions to the User Service project are welcome. If you have any ideas, improvements, or bug fixes, please contact us.