using FleetMan.Application.Registration.Common;
using FleetMan.Application.Registration.RegisterPassengerShip;
using FleetMan.Application.Registration.RegisterTankerShip;
using FleetMan.Contracts.Registration;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FleetMan.Api.Registration;

[Route("[controller]")]
public class ShipsController(IMapper mapper, ISender mediator) : ApiController
{
    private readonly ISender _mediator = mediator;
    private readonly IMapper _mapper = mapper;


    [HttpPost("passenger")]
    public async Task<IActionResult> RegisterPassengerShip(RegisterPassengerShipRequest request)
    {
        var command = _mapper.Map<RegisterPassengerShipCommand>(request);

        var result = await _mediator.Send(command);

        return result.Match(
            contact => Ok(_mapper.Map<RegisterShipResult>(contact)),
            errors => Problem(errors)
        );
    }

    [HttpPost("tanker")]
    public async Task<IActionResult> RegisterTankerShip(RegisterTankerShipRequest request)
    {
        var command = _mapper.Map<RegisterTankerShipCommand>(request);

        var result = await _mediator.Send(command);

        return result.Match(
            contact => Ok(_mapper.Map<RegisterShipResult>(contact)),
            errors => Problem(errors)
        );
    }
}