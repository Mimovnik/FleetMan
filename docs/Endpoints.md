# Endpoints

## `GET /ships`

Returns the list of all registered ships (both passenger and tanker ships).

## `POST /ships`

Registers a ship.

Request body: [RegisterShipRequest.cs](../FleetMan.Contracts/Registration/RegisterShipRequest.cs)

Response body: [RegisterShipResponse.cs](../FleetMan.Contracts/Registration/RegisterShipResponse.cs)

## `POST /ships/${imoNumber}/passengers`

Update the list of passangers for the PassengerShip.

Request body: [UpdatePassengerList.cs](../FleetMan.Contracts/UpdatePassengerListRequest.cs)

Response body: None