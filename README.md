# Kvadratai Project

## Overview

The Kvadratai project is a .NET Core application that manages and analyzes a collection of points in a 2D space. It provides a REST API for adding and retrieving points and includes a service to count the number of squares formed by these points. The project uses MongoDB as the data store and integrates with Prometheus and Grafana for monitoring and visualization.

## Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/products/docker-desktop)
- [MongoDB](https://www.mongodb.com/)
- [Prometheus](https://prometheus.io/)
- [Grafana](https://grafana.com/)

## Setup

### Running the Application
Build and Run Containers:

docker-compose up --build

### Access Services:

- API: http://localhost:8080
- MongoDB: mongodb://localhost:27017
- Prometheus: http://localhost:9090
- Grafana: http://localhost:3000

## Using the API
### Add Points
- Endpoint: POST /Points/AddPoints

- Request Body:

`
[
  { "x": 0, "y": 0 },
  { "x": 1, "y": 1 }
]
`
### Get Points
- Endpoint: GET /Points
  
- Response:
`
[
  { "x": 0, "y": 0 },
  { "x": 1, "y": 1 }
]
`
### Count Squares
- Endpoint: GET /Points/squares

- Response:
`
{
  "count": 2
}
`

## Monitoring
- Prometheus: Accessible at http://localhost:9090
- Grafana: Accessible at http://localhost:3000 (default login: admin/yourpassword)
## Notes
Ensure MongoDB is properly initialized with init-mongo.js.
Customize prometheus.yml for Prometheus configuration.
