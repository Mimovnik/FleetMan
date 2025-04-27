# Use Case: Refuel Tank

## Status

- todo

## Description

Increase the fill of one of the tanks of a tanker ship.

## Input

- `shipImoNumber` - IMO number of the ship to refuel
- `tankNumber` - number of the tank on the ship to refuel
- `fuelAmount` - in liters
- `fuelType` - the type of the incoming fuel

## Output

- `shipImoNumber`
- `tankNumber`
- `fuelInTank` - how much fuel is in the tank after refueling
- `capacity` - how much fuel can be in the tank

## Business Rules / Validation

- ship with given `shipImoNumber` must be already registered
- ship must be of tanker type
- tank with given `tankNumber` must exist
- `fuelAmount` must be positive
- if tank isn't empty, `fuelType` must match the tank's fuel type
- tank's fill cannot be greater then it's capacity

## Domain Logic

- if the tank wasn't empty increase by `amountOfFuel`
- if the tank was empty change it's fuel type

## Errors / Edge Cases

- ship with `shipImoNumber` doesn't exist -> Validation Error
- ship with `shipImoNumber` is not a tanker ship -> Validation error
- ship doesn't have a tank with `tankNumber` -> Validation error
- `fuelAmount` is negative -> Validation error
- `fuelType` doesn't match tank's fuel type -> Domain Error
- the tank would be overfilled after increasing by `amountOfFuel` -> Domain Error
