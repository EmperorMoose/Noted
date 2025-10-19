# Noted

A production-style .NET 8 Web API for managing notes, built to demonstrate full-stack software engineering practices including API design, service architecture, automated testing, Dockerization, CI/CD pipelines, and deployment to Azure cloud infrastructure.

---

## Table of Contents

- [Features](#features)
- [Live Demo](#live-demo)
- [Project Structure](#project-structure)
- [Local Development](#local-development)
- [Docker](#docker)
- [CI/CD Pipeline](#cicd-pipeline)
- [Infrastructure](#infrastructure)
- [Authentication](#authentication)
- [Health Check](#health-check)
- [Purpose](#purpose)
- [License](#license)

## Features

- ASP.NET Core Web API using .NET 8
- JWT-based authentication (simulated)
- PostgreSQL with Entity Framework Core
- Clean architecture with a service layer
- Unit testing with xUnit
- Docker multi-stage builds
- CI/CD using GitHub Actions
- Health checks via `/health`
- Deployment to Azure Container Apps
- Azure Container Registry integration

## Live Demo

The API is deployed to Azure Container Apps.  
Swagger UI is available at:

[noted](https://noted-api.proudtree-36dcd77a.centralus.azurecontainerapps.io/swagger/index.html)

## Project Structure

Noted
- Noted.Api/ # ASP.NET Core Web API
- Noted.Application/ # Services, EF Core DbContext, business logic
- Noted.Shared/ # Shared models and DTOs
- Noted.Tests/ # Unit test project
- Dockerfile # Multi-stage Docker build definition
- github/workflows/ # CI/CD GitHub Actions pipeline

## CI/CD Pipeline
The GitHub Actions pipeline includes the following steps:

Restore, build, and test the project

Build and tag a Docker image

Push the image to Azure Container Registry (ACR)

Authenticate to Azure via a service principal

Update the Azure Container App with the new image

Pipeline is triggered on push or pull_request to the main branch.

## Authentication
The API uses JWT Bearer authentication.

Simulated tokens are generated with a known secret

All protected endpoints require the Authorization header with a bearer token

No real identity provider is used for this demo

## Health Check
A health check endpoint is exposed at:

/health

## Purpose
This project was created to:

Demonstrate proficiency with modern .NET 8 development

Illustrate how to build a clean, testable, layered API

Showcase real-world DevOps skills including Docker, GitHub Actions, and Azure
