# Use Case: Get All Ships

## Description

Fetches a list of all registered ships. Both passenger ships and tanker ships.

## Input

None

## Output

- `list of:`
    - `imoNumber`
    - `name`
    - `length`
    - `width`
    - `type` - the ship type ("PassengerShip" or "TankerShip")

## Business Rules / Validation

None

## Domain Logic

None

## Errors / Edge Cases

- if no ships are registered -> empty list (200 OK)
