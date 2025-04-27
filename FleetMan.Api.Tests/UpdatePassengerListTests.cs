using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using FleetMan.Contracts.UpdatePassengerList;
using FleetMan.Contracts.Registration;
using FleetMan.Domain.Errors;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FleetMan.Api.Tests;

public class UpdatePassengerListTests
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

    private static string Url(string imoNumber) => $"{_baseUrl}/{imoNumber}/passengers";

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

    private static async Task RegisterValidTankerShip(HttpClient client, string imoNumber)
    {
        var request = new RegisterShipRequest(
            ShipType: ShipType.Tanker,
            ImoNumber: imoNumber,
            Name: "Black Pearl",
            Length: 100,
            Width: 20,
            Tanks: [
                new Tank(1000),
                new Tank(1000),
                new Tank(5000),
            ]
        );

        var response = await client.PostAsJsonAsync(_baseUrl, request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("1234567")]
    [InlineData("0000012")]
    [InlineData("0000103")]
    public async Task Given_RegisteredShip_When_UpdatingPassengerList_Then_NoError(string imo)
    {
        var (factory, client) = CreateTestEnvironment();
        await RegisterValidPassengerShip(client, imo);
        // Arrange
        var request = new UpdatePassengerListRequest([
                "John Doe",
                "Jane Smith"
            ]
        );

        // Act
        var response = await client.PostAsJsonAsync(Url(imo), request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [InlineData("1234567")]
    [InlineData("0000012")]
    [InlineData("0000103")]
    public async Task Given_EmptyPassengerName_When_UpdatingPassengerList_Then_BadRequestError(string imo)
    {
        var (factory, client) = CreateTestEnvironment();
        // Arrange
        await RegisterValidPassengerShip(client, imo);

        var request = new UpdatePassengerListRequest([
                "John Doe",
                "",
                "Jane Smith"
            ]
        );

        // Act
        var response = await client.PostAsJsonAsync(Url(imo), request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsWithErrors>();

        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Be("One or more validation errors occurred.");
        problemDetails.Status.Should().Be(400);

        problemDetails.Errors.Should().NotBeNull();

        problemDetails.Errors.Should().ContainSingle(Errors.Passenger.InvalidName.Code);
        problemDetails.Errors[Errors.Passenger.InvalidName.Code].Should().ContainSingle(Errors.Passenger.InvalidName.Description);

    }

    [Theory]
    [InlineData("1234567")]
    [InlineData("0000012")]
    [InlineData("0000103")]
    public async Task Given_ShipDoesntExist_When_UpdatingPassengerList_Then_NotFoundError(string imo)
    {
        (var factory, var client) = CreateTestEnvironment();
        // Arrange
        var request = new UpdatePassengerListRequest([
                "John Doe",
                "Jane Smith"
            ]
        );

        // Act
        var response = await client.PostAsJsonAsync(Url(imo), request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        problemDetails.Should().NotBeNull();
        problemDetails.Status.Should().Be(404);
    }

    [Theory]
    [InlineData("1234567")]
    [InlineData("0000012")]
    [InlineData("0000103")]
    public async Task Given_ShipIsNotPassengerType_When_UpdatingPassengerList_Then_BadRequestError(string imo)
    {
        (var factory, var client) = CreateTestEnvironment();
        // Arrange
        await RegisterValidTankerShip(client, imo);
        var request = new UpdatePassengerListRequest([
                "John Doe",
                "Jane Smith"
            ]
        );

        // Act
        var response = await client.PostAsJsonAsync(Url(imo), request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsWithErrors>();

        problemDetails.Should().NotBeNull();
        problemDetails.Status.Should().Be(400);

        problemDetails.Errors.Should().NotBeNull();

        problemDetails.Errors.Should().ContainSingle(Errors.Ship.NotPassenger.Code);
        problemDetails.Errors[Errors.Ship.NotPassenger.Code].Should().ContainSingle(Errors.Ship.NotPassenger.Description);
    }
}