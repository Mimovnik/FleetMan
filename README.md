# Fleetman

Api for fleet management.

## Prerequisites

- .NET 8.0

## Installation

```
git clone https://github.com/Mimovnik/FleetMan
```

```
dotnet restore
```

## Tests

```
dotnet test
```

## Used frameworks

- ErrorOr - return result type instead of throwing exceptions
- FluentValidation - validate commands and queries
- MediatR - handle commands and queries
- Mapster - request to command/query mapping
- xUnit - unit testing
- FluentAssertions - assertions that are human-readable

## Architecture

This project is my attempt to join Clean Architecture with Vertical Slice Architecture together, while sticking to Test Driven Development methodology.

Namely I've split the project into layers, with individual directory for each one:

- Domain/Business layer (FleetMan.Domain)
- Application layer (FleetMan.Application)
- Infrastructure layer (FleetMan.Infrastructure)
- Presentation layer (FleetMan.Api and FleetMan.Contracts)

But inside of each layer I've segregated the classes feature-wise in the Vertical Slice spirit.
eg. directory `Registration` contains features that register ships (`RegisterPassengerShip` and `RegisterTankerShip`)


I've tried to write tests first and only then implement the methods.


I decided on such architecture mix mainly for the sake of learning but also to extract the pros of these architectures.


Keeping Domain layer independent of other layers makes it future-proof.
It makes it possible to change the api, database management system, application flow or other non-domain code without interfering with the domain layer.

The feature segregation makes it easier to locate classes operating within certain functionality thus faster development and easier onboarding of new developers.
