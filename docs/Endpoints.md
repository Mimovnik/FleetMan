# Endpoints

## `GET /ships`

Returns the list of all registered ships (both passenger and tanker ships).

## `POST /ships/passenger`

Registers a passenger ship.

Request: [RegisterPassengerShipRequest.cs](../FleetMan.Contracts/Registration/RegisterPassengerShipRequest.cs)

Response: [RegisterShipResponse.cs](../FleetMan.Contracts/Registration/RegisterShipResponse.cs)

## `POST /ships/tanker`

Registers a tanker ship.

Request: [RegisterTankerShipRequest.cs](../FleetMan.Contracts/Registration/RegisterTankerShipRequest.cs)

Response: [RegisterShipResponse.cs](../FleetMan.Contracts/Registration/RegisterShipResponse.cs)
