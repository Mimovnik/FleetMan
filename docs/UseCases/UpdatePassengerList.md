# Use Case: Update Passenger List

## Status

- wip

## Description
Update the list of passengers assigned to a registered passenger ship.
This operation replaces the current list with a new one.

## Input

- `passengers` - new list of passengers where each passenger has:
    - `name`

## Output

None

## Business Rules / Validation

- each `passegers:name` must not be empty

## Domain Logic

- if the ship exists and is a passenger ship, update its passengers list with the provided names
- completely replace the old list (no merging)

## Errors / Edge Cases

- Empty `passengers:name` -> Validation error
