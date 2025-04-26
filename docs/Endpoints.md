# Endpoints

## `GET /ships`

Returns the list of all registered ships (both passenger and tanker ships).

## `POST /ships`

Registers a ship.

Request body: [RegisterShipRequest.cs](../FleetMan.Contracts/Registration/RegisterShipRequest.cs)

Response body: [RegisterShipResponse.cs](../FleetMan.Contracts/Registration/RegisterShipResponse.cs)
