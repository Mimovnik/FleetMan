using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using FleetMan.Contracts.RefuelTank;
using FleetMan.Contracts.Registration;
using FleetMan.Domain.Errors;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FleetMan.Api.Tests;

public class DrainTankTests
{
    private const string _baseUrl = "/ships";

    private static (WebApplicationFactory<Program> Factory, HttpClient Client) CreateTestEnvironment()
    {
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        return (factory, client);
    }

    public class ProblemDetailsWithErrors : ProblemDetails
    {
        [JsonPropertyName("errors")]
        public required Dictionary<string, string[]> Errors { get; set; }
    }

    private static string DrainUrl(string imoNumber, string tankNumber) => $"{_baseUrl}/{imoNumber}/tanks/{tankNumber}/drain";
    private static string RefuelUrl(string imoNumber, string tankNumber) => $"{_baseUrl}/{imoNumber}/tanks/{tankNumber}/refuel";

    private static async Task RegisterValidPassengerShip(HttpClient client, string imoNumber)
    {
        var request = new RegisterShipRequest(
            ShipType: ShipType.Passenger,
            ImoNumber: imoNumber,
            Name: "Black Pearl",
            Length: 100,
            Width: 20
        );

        var response = await client.PostAsJsonAsync(_baseUrl, request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private static async Task RegisterValidTankerShip(
        HttpClient client,
        string imoNumber,
        List<Tank> tanks)

    {
        var request = new RegisterShipRequest(
            ShipType: ShipType.Tanker,
            ImoNumber: imoNumber,
            Name: "Black Pearl",
            Length: 100,
            Width: 20,
            Tanks: tanks
        );

        var response = await client.PostAsJsonAsync(_baseUrl, request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("1234567")]
    [InlineData("0000012")]
    [InlineData("0000103")]
    public async Task Given_RegisteredTankerShip_When_Refueling_NoError(string imo)
    {
        var (factory, client) = CreateTestEnvironment();
        // Arrange
        await RegisterValidTankerShip(client, imo, [
                new Tank(1000),
                new Tank(1000),
                new Tank(5000),
            ]
        );
        var request = new RefuelTankRequest(
            FuelAmount: 100,
            FuelType: "Diesel"
        );


        // Act
        var response = await client.PostAsJsonAsync(DrainUrl(imo, "1"), request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [InlineData("1234567")]
    [InlineData("0000012")]
    [InlineData("0000103")]
    public async Task Given_ShipDoesntExist_When_Draining_Then_ShipNotFoundError(string imo)
    {
        var (factory, client) = CreateTestEnvironment();
        // Act
        var response = await client.PostAsync(DrainUrl(imo, "5"), null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Be(Errors.Ship.NotFound.Description);
        problemDetails.Status.Should().Be(404);
    }

    [Theory]
    [InlineData("1234567")]
    [InlineData("0000012")]
    [InlineData("0000103")]
    public async Task Given_ShipIsNotATanker_When_Draining_Then_NotTankerError(string imo)
    {
        var (factory, client) = CreateTestEnvironment();
        // Arrange
        await RegisterValidPassengerShip(client, imo);
        // Act
        var response = await client.PostAsync(DrainUrl(imo, "4"), null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsWithErrors>();

        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Be("One or more validation errors occurred.");
        problemDetails.Status.Should().Be(400);

        problemDetails.Errors.Should().NotBeNull();

        problemDetails.Errors.Should().ContainSingle(Errors.Ship.NotTanker.Code);
        problemDetails.Errors[Errors.Ship.NotTanker.Code].Should().ContainSingle(Errors.Ship.NotTanker.Description);
    }

    [Theory]
    [InlineData("1234567")]
    [InlineData("0000012")]
    [InlineData("0000103")]
    public async Task Given_TankDoesntExist_When_Draining_Then_TankNotFoundError(string imo)
    {
        var (factory, client) = CreateTestEnvironment();
        // Arrange
        await RegisterValidTankerShip(client, imo, [
                new Tank(1000),
                new Tank(1000),
                new Tank(5000),
            ]
        );

        // Act
        var response = await client.PostAsync(DrainUrl(imo, "4"), null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Be(Errors.Tank.NotFound.Description);
        problemDetails.Status.Should().Be(404);
    }


    [Fact]
    public async Task Given_TankWithFuel_When_RefuelingWithDifferentFuelTypeAfterDraining_Then_SecondFillMatches()
    {
        var (factory, client) = CreateTestEnvironment();
        // Arrange
        string imo = "1234567";
        await RegisterValidTankerShip(client, imo, [
                new Tank(1000),
                new Tank(1000),
                new Tank(5000),
            ]
        );
        var requestDieselPreDrain = new RefuelTankRequest(
            FuelAmount: 100,
            FuelType: "Diesel"
        );

        string tankNumber = "1";
        await client.PostAsJsonAsync(RefuelUrl(imo, tankNumber), requestDieselPreDrain);

        var requestHeavyFuelPostDrain = new RefuelTankRequest(
            FuelAmount: 123.45f,
            FuelType: "HeavyFuel"
        );
        await client.PostAsync(DrainUrl(imo, tankNumber), null);
        // Act

        var response = await client.PostAsJsonAsync(RefuelUrl(imo, tankNumber), requestHeavyFuelPostDrain);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<RefuelTankResponse>();
        result.Should().NotBeNull();
        result.FuelAmount.Should().Be(123.45f);
        result.FuelType.Should().Be("HeavyFuel");
    }
}