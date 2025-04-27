namespace FleetMan.Contracts.UpdatePassengerList;

public record UpdatePassengerListRequest(
    List<string> Passengers
);