![Flow Diagram](Assets/flow.svg)

# Technical Architecture Overview: Purchasing & Shipping Order System

## 📑 Table of Contents

| Section | Description |
|--------|-------------|
| [1. Architectural Approach](#1-architectural-approach) | Design principles and service setup |
| [2. Service Roles](#2-service-roles) | Responsibilities of Purchase and Shipping Order services |
| [3. Communication Mechanisms](#3-communication-mechanisms) | gRPC and event-driven communication |
| [4. Outbox Pattern](#4-outbox-pattern-reliability-strategy) | Ensuring reliable messaging |
| [5. Observability and Infrastructure](#5-observability-and-infrastructure) | Logging and deployment infrastructure |
| [6. Testing](#6-testing) | Testing Coverage |
| [7. Upcoming Work](#7-upcoming-work) | Planned enhancements |
| [Accessing APIs](#-accessing-apis) | Swagger endpoints |
| [Health Endpoints](#health-endpoints) | Health Status |
---

## 1. Architectural Approach

This system adopts a **Domain-Driven Design (DDD)** and **Clean Architecture** strategy, with **modular bounded contexts** for Purchasing and Shipping. Services are fully decoupled and communicate through **asynchronous messaging** and **synchronous gRPC**, enabling scalability, resilience, and runtime flexibility.

Each service is:

- A **standalone Dockerized process**.
- Owns its **data storage** (SQL Server).
- **Version-tolerant** and **extensible**, allowing runtime changes with minimal impact.

---

## 2. Service Roles

### 🛒 **Purchase Order Service**

* Handles creation, approval, and retrieval of purchase orders.
* Exposes a **REST API** for client communication.
* Hosts a **gRPC server** to support internal validation requests from the Shipping Order Service.
* **Consumes domain events** (e.g., `ShippingOrderCreated`, `ShippingOrderClosed`) to update order states based on shipping progress.

### 📦 **Shipping Order Service**

* Manages the lifecycle of shipping orders (create, close).
* Exposes a **REST API** to external clients.
* **Acts as a gRPC client**, calling the Purchase Order Service to validate PO data before creating shipments.
* **Publishes domain events** to notify other services of shipping status changes.

## **3. Communication Mechanisms**

### 🔁 **gRPC (Internal Synchronous Communication)**

* **Use case**: When the Shipping Order Service needs to **validate Purchase Order data** before creating a shipment.
* **Benefits**:

  * Low latency and high efficiency.
  * Strong typing and schema enforcement.
  * Ideal for internal microservice communication.

### 📩 **Message Broker (Asynchronous Communication)**

* **RabbitMQ** is used as the central **event bus**.
* The **Shipping Order Service publishes domain events** like:

  * `ShippingOrderCreated`
  * `ShippingOrderClosed`
* The **Purchase Order Service listens and consumes these events** to perform state updates without tight coupling.
* Enables **event-driven workflows** across service boundaries.

---

## **4. Outbox Pattern (Reliability Strategy)**

* Implemented in the **Shipping Order Service**.
* Ensures **event delivery reliability** by:

  * Persisting domain events in an **Outbox table** alongside business entities.
  * A background listener reads the Outbox and **publishes events to RabbitMQ**, guaranteeing eventual delivery.
* Solves the **dual-write problem** and maintains **data-event consistency**.

## 5. Observability and Infrastructure

### 🔍 Centralized Logging with Elasticsearch + Kibana

- Logs pushed to **Elastic Cloud**.
- Use **Kibana** for:
  - Real-time service monitoring.
  - Tracing and debugging.
  - Error visualization.

### 🐳 Dockerized Deployment

- All services and infrastructure run in **Docker containers**.


## 6. Testing

The solution includes well-structured test projects to ensure code quality and validate architectural and domain correctness:

| Project                                 | Purpose                                                                                                                                      |
| --------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------- |
| `PurchasingOrder.Application.UnitTests` | Verifies application layer logic such as use cases, commands, and query handlers.                                                            |
| `PurchasingOrder.ArchitectureTests`     | Uses tools like [NetArchTest](https://github.com/BenMorris/NetArchTest) or similar to validate architectural rules and layering constraints. |
| `PurchasingOrder.Domain.UnitTests`      | Ensures correctness of domain entities, value objects, and domain services through isolated unit tests.                                      |

## 7. Upcoming Work

### 🛡 API Gateway

- Central access point for services.
- Handles routing, auth, throttling.

### 💥 Resilience Mechanisms

- Circuit Breaker
- Retry logic
- Fallback & timeout strategies

### 🖥 Simple GUI Interface

-  UI for:
  - Managing PO & SHO lifecycle.
  - Tracking document status visually.

---

### 🌐 Accessing APIs

Once the services are running, you can explore their REST APIs using Swagger:

- **Purchase Order Service**: [https://localhost:6060/swagger/index.html](https://localhost:6060/swagger/index.html)
- **Shipping Order Service**: [https://localhost:6160/swagger/index.html](https://localhost:6160/swagger/index.html)

## Health Endpoints

Each microservice exposes a health check endpoint to verify the status of dependencies such as the database, gRPC connections, and message broker:

| Service                | Health URL                     | Checks                                         |
| ---------------------- | ------------------------------ | ---------------------------------------------- |
| Purchase Order Service | `http://localhost:6060/health` | SQL Server, RabbitMQ, gRPC server readiness    |
| Shipping Order Service | `http://localhost:6160/health` | SQL Server, RabbitMQ, gRPC client connectivity |

✅ Use these URLs to integrate with monitoring tools or to manually inspect service health.
