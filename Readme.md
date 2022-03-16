# Order Service - Monolith [![Build Job](https://github.com/dominioncfg/order-service/actions/workflows/dotnet.yml/badge.svg)](https://github.com/dominioncfg/order-service/actions/workflows/dotnet.yml)

Order service is clean architecture reference application built with .Net6.0.

## Infrastructure

- Postgresql for persistence.
- RabbitMQ as a message transport.
- Docker and Docker compose for running the infrastructure for local development.

## Tools & libraries

- MediatR for reducing dependency with the actual host.
- AutoMapper for easy mapping of objects between different layers.
- MassTransit for abstracting from the message transport (RabbitMQ in this case).
- FluentValidation for request validation.
- ProblemDetails for standard error response from the web api (rfc7807).
- EntityFrameworkCore for generating DB schemas as well as easy persisting objects in db.
- Dapper for faster queries.
- Xunit as a test framework.
- FluentAssertions for more readable assertions on tests.
- Swagger for making easier to test all the endpoints.

## Some patterns used

- DDD for having a business first design.
- CQRS for reads and writes separation.
- Builder Design Pattern heavily used on tests.

## Tests

This project contains both Unit and Integration Tests.

## Example use case

The requirements are pretty straight forward, we have a system that handles order creation and those orders can change their status in the fallowing manner:

- Orders is Submitted (Created) => Orders is Paid => Order is Shipped.
- In any given point in time orders can be cancelled to.
- Whenever any of these changes happens, messages are published to inform potential external systems.

The use cases are not a real live example, there is just enough information for demonstrating some DDD concepts like Aggregates, Entities, Value Objects, repositories etc.

## Notes

Although the example includes messaging is not intended to be a reference application for microservices, as its missing important features like ApiGateways, BFF, Telemetry, Transient Fault Handling etc.

Also note that one of the Project, OrderService.Consumer should not be considered part of example is only there for illustrating how a possible consumer should look like, its missing important features like testing.

## Working with the example

After downloading the repo, the process of starting the app is really straight forward.

1- Start the docker dependencies:

```bash
docker-compose -p order-service-dev up --build --detach
```

2- Open the solution on VS 2022 and start both OrderService.Host and OrderService.Consumer.

## CI

The example also includes a simple Github Action for checking pull request making sure that the code compiles and the test are valid before merging but is missing other features like test reporting, artifact generation, etc.
