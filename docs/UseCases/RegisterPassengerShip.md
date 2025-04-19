# Use Case: Register Passenger Ship

## Description
Register a new passenger ship ship details and an empty passenger list.

## Input

- `imoNumber` – 7-digit ship identifier (see https://en.wikipedia.org/wiki/IMO_number)
- `name`
- `length`
- `width`

## Output
- `imoNumber`
- `name`

## Business Rules / Validation

- `imoNumber`:
    - must be unique
    - must be 7-digits long
    - has valid checksum
- `length` must be greater then 0
- `width` must be greater then 0

## Domain Logic

- initialize the ship's passenger list to empty

## Errors / Edge Cases

- Duplicate `imoNumber` -> Validation error
- Invalid checksum of `imoNumber` → Validation error
- Negative `length` or `width` → Validation error
