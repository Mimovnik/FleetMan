using ErrorOr;
using MediatR;

namespace FleetMan.Application.UpdatePassengerList;

public record UpdatePassengerListCommand(
    string ShipImoNumber,
    IEnumerable<string> PassengerNames
) : IRequest<ErrorOr<Unit>>;