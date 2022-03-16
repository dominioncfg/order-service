# Order Service Clean Architecture

Order service is clean architecture reference application built with .Net6.0.

## Infraestucture

- Postgresql for persistance.
- RabbitMQ as a message transport.
- Docker and Docker compose for running the infraestructure for local development

## Tools & libraries

- MediatR for reducing dependency with the actual host.
- AutoMapper for easy mapping of objects between different layers.
- MassTransit for abstracting from the message transport (RabbitMQ in this case).
- FluentValidation for request validation.
- ProblemDetails for standard error response from the web api (rfc7807).
- EntityFrameworkCore for generating db schemas as well as easy persisting objects in db.
- Dapper for faster queries.
- Xunit as a test framework.
- FluentAssertions for more readable assertions on test.
- Swagger for making easier to test all the enpoints.

## Some patterns used

- DDD for having a bussiness first design.
- CQRS for reads and writes separation.
- Builder Design Pattern heavily used on tests,

## Tests

This projects contains both Unit and Integration Tests.

## Example use case

The requirements are pretty straight foward, we have a systems that handles order creation and manipulation and those orders can change their status in the fallowing manner:

Orders is Submitted (Created) => Orders is Paid => Order is Shipped.
and in a any given point in time Orders can be cancelled to.

Whenever any of these changes happens messages are published to inform possible external system

The use cases are not a real live examples they contain just enough information for demostrating some DDD conceps like Aggregates, Entities, Value Objects, repositories etc.

## Notes

Although the example includes messaging is not intended to be a reference application for microservices, as it have missing important features like ApiGateways, BFF, Telemetry, Transient Fault Handling etc.

Also note that one of the Project, OrderService.Consumer should not be considered part of example is only there for ilustrating how a possible consumer should look like, its missing important features like testing.

## Working with the example

After downloading the repo the process of starting the app is really straight forward.

1- Start the docker dependencies:

```bash
docker-compose -p order-service-dev up --build --detach
```

2- Open the solution on VS 2022 and start both OrderService.Host and OrderService.Consumer.

## CI

The example also includes a simple github action for checking pull request making sure that the code compiles and the test are valid before merging but is missing other features like test reporting, artifact generation etc.
