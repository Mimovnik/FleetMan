# Endpoints

## `GET /ships`

Returns the list of all registered ships (both passenger and tanker ships).

## `POST /ships`

Registers a ship.

Request body: [RegisterShipRequest.cs](../FleetMan.Contracts/Registration/RegisterShipRequest.cs)

Response body: [RegisterShipResponse.cs](../FleetMan.Contracts/Registration/RegisterShipResponse.cs)

## `POST /ships/${imoNumber}/passengers`

Update the list of passangers for the PassengerShip.

Request body: [UpdatePassengerListRequest.cs](../FleetMan.Contracts/UpdatePassengerListRequest.cs)

Response body: None

## `POST /ships/${imoNumber}/tanks/${tankNumber}`

Refuel a specific tank on a TankerShip.

Request body: [RefuelTankRequest.cs](../FleetMan.Contracts/RefuelTank/RefuelTankRequest.cs)

Response body: [RefuelTankResponse.cs](../FleetMan.Contracts/RefuelTank/RefuelTankResponse.cs)