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

public class RefuelTankTests
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

    private static string Url(string imoNumber, string tankNumber) => $"{_baseUrl}/{imoNumber}/tanks/{tankNumber}/refuel";

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
        var response = await client.PostAsJsonAsync(Url(imo, "1"), request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }


    [Theory]
    [InlineData("-2")]
    [InlineData("")]
    [InlineData("number")]
    public async Task Given_InvalidTankNumber_When_Refueling_Then_ValidationError(string tankNumber)
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
        var request = new RefuelTankRequest(
            FuelAmount: 100,
            FuelType: "Diesel"
        );

        // Act
        var response = await client.PostAsJsonAsync(Url(imo, tankNumber), request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("1234567")]
    [InlineData("0000012")]
    [InlineData("0000103")]
    public async Task Given_ShipDoesntExist_When_Refueling_Then_ShipNotFoundError(string imo)
    {
        var (factory, client) = CreateTestEnvironment();
        // Arrange
        var request = new RefuelTankRequest(
            FuelAmount: 100,
            FuelType: "Diesel"
        );

        // Act
        var response = await client.PostAsJsonAsync(Url(imo, "5"), request);

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
    public async Task Given_ShipIsNotATanker_When_Refueling_Then_NotTankerError(string imo)
    {
        var (factory, client) = CreateTestEnvironment();
        // Arrange
        await RegisterValidPassengerShip(client, imo);
        var request = new RefuelTankRequest(
            FuelAmount: 100,
            FuelType: "Diesel"
        );

        // Act
        var response = await client.PostAsJsonAsync(Url(imo, "4"), request);

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
    public async Task Given_TankDoesntExist_When_Refueling_Then_TankNotFoundError(string imo)
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
        var response = await client.PostAsJsonAsync(Url(imo, "4"), request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Be(Errors.Tank.NotFound.Description);
        problemDetails.Status.Should().Be(404);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-1123.5)]
    public async Task Given_NegativeAmount_When_Refueling_Then_InvalidRefuelAmountError(int amount)
    {
        var (factory, client) = CreateTestEnvironment();
        // Arrange
        var imo = "1234567";
        await RegisterValidTankerShip(client, imo, [
                new Tank(1000),
                new Tank(1000),
                new Tank(5000),
            ]
        );
        var request = new RefuelTankRequest(
            FuelAmount: amount,
            FuelType: "Diesel"
        );

        // Act
        var response = await client.PostAsJsonAsync(Url(imo, "4"), request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsWithErrors>();

        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Be("One or more validation errors occurred.");
        problemDetails.Status.Should().Be(400);

        problemDetails.Errors.Should().NotBeNull();

        problemDetails.Errors.Should().ContainSingle(Errors.Tank.InvalidRefuelAmount.Code);
        problemDetails.Errors[Errors.Tank.InvalidRefuelAmount.Code].Should().ContainSingle(Errors.Tank.InvalidRefuelAmount.Description);
    }

    [Fact]
    public async Task Given_TankWithFuel_When_RefuelingWithDifferentFuelType_Then_FuelMismatchError()
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
        var requestDiesel = new RefuelTankRequest(
            FuelAmount: 100,
            FuelType: "Diesel"
        );

        string tankNumber = "1";
        await client.PostAsJsonAsync(Url(imo, tankNumber), requestDiesel);

        var requestHeavyFuel = new RefuelTankRequest(
            FuelAmount: 100,
            FuelType: "HeavyFuel"
        );
        // Act

        var response = await client.PostAsJsonAsync(Url(imo, tankNumber), requestHeavyFuel);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsWithErrors>();

        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Be("One or more validation errors occurred.");
        problemDetails.Status.Should().Be(400);

        problemDetails.Errors.Should().NotBeNull();

        problemDetails.Errors.Should().ContainSingle(Errors.Tank.FuelMismatch.Code);
        problemDetails.Errors[Errors.Tank.FuelMismatch.Code].Should().ContainSingle(Errors.Tank.FuelMismatch.Description);
    }

    [Theory] // firstFill must be less then or equal to capacity
    [InlineData(1, 100, 100)]
    [InlineData(100, 1, 100)]
    [InlineData(10, 10000, 100)]
    [InlineData(500, 10000, 500.25)]
    [InlineData(500.1, 1, 500.25)]
    public async Task Given_TankWithFuel_When_RefuelingWithTooMuchFuel_Then_OverfillError(float firstFill, float secondFill, float capacity)
    {
        var (factory, client) = CreateTestEnvironment();
        // Arrange
        string imo = "1234567";
        await RegisterValidTankerShip(client, imo, [
                new Tank(capacity),
            ]
        );
        var requestDiesel = new RefuelTankRequest(
            FuelAmount: firstFill,
            FuelType: "Diesel"
        );

        string tankNumber = "1";
        await client.PostAsJsonAsync(Url(imo, tankNumber), requestDiesel);

        var requestHeavyFuel = new RefuelTankRequest(
            FuelAmount: secondFill,
            FuelType: "HeavyFuel"
        );
        // Act

        var response = await client.PostAsJsonAsync(Url(imo, tankNumber), requestHeavyFuel);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsWithErrors>();

        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Be("One or more validation errors occurred.");
        problemDetails.Status.Should().Be(400);

        problemDetails.Errors.Should().NotBeNull();

        problemDetails.Errors.Should().ContainSingle(Errors.Tank.TankWouldOverflow.Code);
        problemDetails.Errors[Errors.Tank.TankWouldOverflow.Code].Should().ContainSingle(Errors.Tank.TankWouldOverflow.Description);
    }
}