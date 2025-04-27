using ErrorOr;
using FleetMan.Application.RefuelTank;
using FleetMan.Application.Registration.Common;
using FleetMan.Application.Registration.RegisterPassengerShip;
using FleetMan.Application.Registration.RegisterTankerShip;
using FleetMan.Application.UpdatePassengerList;
using FleetMan.Contracts.RefuelTank;
using FleetMan.Contracts.Registration;
using FleetMan.Contracts.UpdatePassengerList;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FleetMan.Api.Controllers;

[Route("[controller]")]
public class ShipsController(IMapper mapper, ISender mediator) : ApiController
{
    private readonly ISender _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    private async Task<IActionResult> RegisterShipAsync<TCommand>(TCommand command)
        where TCommand : IRequest<ErrorOr<RegisterShipResult>>
    {
        var result = await _mediator.Send(command);

        return result.Match(
            ship => Ok(_mapper.Map<RegisterShipResult>(ship)),
            errors => Problem(errors)
        );
    }

    [HttpPost]
    public async Task<IActionResult> RegisterShip(RegisterShipRequest request)
    {
        if (request.ShipType == ShipType.Passenger)
        {
            var command = _mapper.Map<RegisterPassengerShipCommand>(request);
            return await RegisterShipAsync(command);
        }
        else if (request.ShipType == ShipType.Tanker)
        {
            var command = _mapper.Map<RegisterTankerShipCommand>(request);
            return await RegisterShipAsync(command);
        }
        else
        {
            return BadRequest("Invalid ship type");
        }
    }

    [HttpPost("{imoNumber}/passengers")]
    public async Task<IActionResult> UpdatePassengerList(string imoNumber, UpdatePassengerListRequest request)
    {
        var command = _mapper.Map<UpdatePassengerListCommand>((imoNumber, request));

        var result = await _mediator.Send(command);

        return result.Match(
            _ => NoContent(),
            errors => Problem(errors)
        );
    }

    [HttpPost("{imoNumber}/tanks/{tankNumber}")]
    public async Task<IActionResult> RefuelTank(string imoNumber, int tankNumber, RefuelTankRequest request)
    {
        var command = _mapper.Map<RefuelTankCommand>((imoNumber, tankNumber, request));

        var result = await _mediator.Send(command);

        return result.Match(
            result => Ok(_mapper.Map<RefuelTankResult>(result)),
            errors => Problem(errors)
        );
    }
}