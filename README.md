# DigitStore

**Disclaimer:** This project is currently in development. All tokens, passwords and secrets visible in this repository are used for local development and testing purposes only.

#### Modular monolith app for an automated digital products store.

## Architecture and Communication
The project is built using a **Modular Monolith** architecture combined with the **Vertical Slice Architecture**. Modules are divided into `ModuleName.Module` and `ModuleName.Contracts` projects, communicating using two approaches:


- **Asynchronous Communication:** Event-driven approach using **MassTransit** and **RabbitMQ**. Modules publish and subscribe to events.
- **Synchronous Communication:** Handled through exposed public interfaces when immediate consistency is needed. Modules do not reference each other's internal logic, relying only on shared abstractions.

### Shared Projects

- **[Shared.Abstractions](https://github.com/lukgos/DigitStore/tree/main/src/Shared/Shared.Abstractions)** - Shared abstractions available for every module
- **[Shared.Infrastructure](https://github.com/lukgos/DigitStore/tree/main/src/Shared/Shared.Infrastructure)** - Shared implementation hidden from every module
- **[Shared.EntityFramework](https://github.com/lukgos/DigitStore/tree/main/src/Shared/Shared.EntityFramework)** - Shared settings for modules which use Entity Framework Core
- **[Shared.Application](https://github.com/lukgos/DigitStore/tree/main/src/Shared/Shared.Application)** - Shared components for every module to build endpoints

### Available Modules

**[Account](https://github.com/lukgos/DigitStore/tree/main/src/Modules/Account/Account.Module)** - Secure authentication based on JWT tokens stored in HttpOnly cookies
- [Available actions:](https://github.com/lukgos/DigitStore/tree/main/src/Modules/Account/Account.Module/Features)
    - Sign up and sign in
    - Get information about the authenticated user
---
**[Customer](https://github.com/lukgos/DigitStore/tree/main/src/Modules/Customer/Customer.Module)** - Customer profile, preferences and details
 - [Available actions:](https://github.com/lukgos/DigitStore/tree/main/src/Modules/Customer/Customer.Module/Consumers/AccountCreatedConsumer.cs)
    - Consumes the account creation event. Currently logs information about creating a customer profile
---
**[Catalog](https://github.com/lukgos/DigitStore/tree/main/src/Modules/Catalog/Catalog.Module)** - Products management
 - [Available actions:](https://github.com/lukgos/DigitStore/tree/main/src/Modules/Catalog/Catalog.Module/Features)
    - Create and delete categories
    - Create and update products
    - Get product details
---
**[Search](https://github.com/lukgos/DigitStore/tree/main/src/Modules/Search/Search.Module)** - Fast product search based on OpenSearch
 - [Available actions:](https://github.com/lukgos/DigitStore/tree/main/src/Modules/Search/Search.Module/Features)
    - Search products based on query string
    - Consumes product creation and update events to create or update an equivalent product in the OpenSearch database for fast searching
---
**[Order](https://github.com/lukgos/DigitStore/tree/main/src/Modules/Order/Order.Module)** - Order lifecycle management
 - [Available actions:](https://github.com/lukgos/DigitStore/tree/main/src/Modules/Order/Order.Module/Features)
    - Create an order from a single product
    - Consumes payment completed and order delivered events to set the order status
---
**[Payment](https://github.com/lukgos/DigitStore/tree/main/src/Modules/Payment/Payment.Module)** - Handling payment processing
 - [Available actions:](https://github.com/lukgos/DigitStore/tree/main/src/Modules/Payment/Payment.Module/Consumers/OrderCreatedConsumer.cs)
    - Consumes the order creation event and simulates the payment process
---
**[Distribution](https://github.com/lukgos/DigitStore/tree/main/src/Modules/Distribution/Distribution.Module)** - Automated distribution of digital products
 - [Available actions:](https://github.com/lukgos/DigitStore/tree/main/src/Modules/Distribution/Distribution.Module/Consumers/OrderPreparedForDeliveryConsumer.cs)
    - Consumes the order paid event. Currently logs information about product delivery

### Planned Modules

- **Basket** - For shopping cart to buy multiple products at once
- **Analytics** - For sales monitoring and behavior analysis
- **Promotion** - For discount codes and discount strategies
- **Review** - For products ratings and customer feedback
- **Notification** - For e-mail notifications
- **Wishlist** - For product and price monitoring

## Tech Stack

- [.NET10](https://github.com/dotnet) - Platform
- [Entity Framework Core](https://github.com/dotnet/efcore) - ORM
- [Docker](https://github.com/docker) - Containerization for local development 
- [PostgreSQL](https://github.com/postgres/postgres) - Main database, used with JSON support in the Catalog module
- [OpenSearch](https://opensearch.org) - Database used in Search module for fast searching
- [GitHub Actions](https://github.com/lukgos/DigitStore/blob/main/.github/workflows/dotnet.yml) - For continuous integration
- [RabbitMQ](https://github.com/rabbitmq) - Asynchronous messaging broker
- [MassTransit](https://masstransit-project.com) - Asynchronous messaging library
- [OpenTelemetry](https://opentelemetry.io) - For collecting telemetry data
- [Jaeger](https://www.jaegertracing.io) - For tracking requests
- [Prometheus](https://prometheus.io) - For collecting system metrics
- [Grafana](https://grafana.com) - Visual dashboards and monitoring for metrics collected by Prometheus
- [Seq](https://datalust.co/seq) - For structured logging
- [xUnit](https://xunit.net/) - For unit and integration tests

## How to run

**Requirements:**
- Docker
- .NET 10 SDK

**Running the environment:**

1. Clone the repository and navigate to the `/tools/docker/` directory containing the `docker-compose.yml` file.
2. Run the `docker compose` command to start the services:
   ```bash
   docker compose up -d --build
   ```
   You can also run the API in the container using the profile:
   ```bash
   docker compose --profile docker up -d --build
   ```