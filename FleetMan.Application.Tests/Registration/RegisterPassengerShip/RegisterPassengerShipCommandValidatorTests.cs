using FleetMan.Application.Registration.RegisterPassengerShip;
using FleetMan.Application.Tests.Registration.Common;

namespace FleetMan.Application.Tests.Registration.RegisterPassengerShip;

public class RegisterPassengerShipCommandValidatorTests : RegisterShipCommandValidatorTests<RegisterPassengerShipCommand, RegisterPassengerShipCommandValidator>
{
    protected override RegisterPassengerShipCommand CreateCommand(string imoNumber, string name, float length, float width)
    {
        return new RegisterPassengerShipCommand(imoNumber, name, length, width);
    }
}

