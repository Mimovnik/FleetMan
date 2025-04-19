# Use Case: Register Tanker Ship

## Description
Register a new tanker ship with ship details and tanks. The number of tanks as well as capacity of each tank should be specified.

## Input

- `imoNumber` – 7-digit ship identifier (see https://en.wikipedia.org/wiki/IMO_number)
- `name`
- `length`
- `width`
- `tanks` - list of tanks (each tank has it's capacity, fuel type and fill)

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
- `tanks` list must be non-empty
- `tanks` fuel type of each tank must be either diesel or heavy fuel
- `tanks` capacity of each tank must be greater then 0

## Domain Logic

- initialize the tanks' fill to empty (start with empty tanks)

## Errors / Edge Cases

- Duplicate `imoNumber` -> Validation error
- Invalid checksum of `imoNumber` → Validation error
- Negative `length` or `width` → Validation error
- Empty `tanks` list -> Validation error
- Fuel of any tank is neither diesel or heavy fuel -> Validation error
- Capacity of any tank is less or equal to 0 -> Validation error
