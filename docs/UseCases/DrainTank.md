# Use Case: Refuel Tank

## Status

- wip

## Description

Empty one of the tanks of a tanker ship.

## Input

- `shipImoNumber` - IMO number of the ship to refuel
- `tankNumber` - number of the tank on the ship to refuel

## Output

None

## Business Rules / Validation

- ship with given `shipImoNumber` must be already registered
- ship must be of tanker type
- tank with given `tankNumber` must exist

## Domain Logic

- zero the tank's fill
- make the tank accept other fuel types

## Errors / Edge Cases

- ship with `shipImoNumber` doesn't exist -> Validation Error
- ship with `shipImoNumber` is not a tanker ship -> Validation error
- ship doesn't have a tank with `tankNumber` -> Validation error
