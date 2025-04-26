using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using FleetMan.Contracts.Registration;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FleetMan.Api.Tests.Registration;

public class RegisterTankerShipTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    private const string _baseUrl = "/ships/tanker";

    public class ProblemDetailsWithErrors : ProblemDetails
    {
        [JsonPropertyName("errors")]
        public required Dictionary<string, string[]> Errors { get; set; }
    }

    [Fact]
    public async Task Given_ValidInput_When_RegisteringNewShip_Then_ReturnsResult()
    {
        // Arrange
        var request = new RegisterTankerShipRequest(
           ImoNumber: "1234567",
           Name: "Black Pearl",
           Length: 100,
           Width: 20,
           Tanks: [
               new Tank("Diesel", 1024.123f),
               new Tank("HeavyFuel", 10),
               new Tank("Diesel", 10000),
               new Tank("HeavyFuel", 124.5f),
           ]
        );

        // Act
        var response = await _client.PostAsJsonAsync(_baseUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseData = await response.Content.ReadFromJsonAsync<RegisterShipResponse>();

        responseData.Should().NotBeNull();
        responseData.ImoNumber.Should().Be(request.ImoNumber);
        responseData.Name.Should().Be(request.Name);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(-10, 0)]
    [InlineData(0, -1023.4)]
    [InlineData(-123.4, -432.1)]
    public async Task Given_InvalidBothWidthAndLength_When_RegisteringNewShip_Then_BadRequestWithErrors(float invalidLength, float invalidWidth)
    {
        // Arrange
        var request = new RegisterTankerShipRequest(
           ImoNumber: "0000000",
           Name: "Titanic",
           Length: invalidLength,
           Width: invalidWidth,
           Tanks: [
                new Tank("Diesel", 1024.123f),
                new Tank("HeavyFuel", 10),
                new Tank("Diesel", 10000),
                new Tank("HeavyFuel", 124.5f),
              ]
        );

        // Act
        var response = await _client.PostAsJsonAsync(_baseUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsWithErrors>();

        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Be("One or more validation errors occurred.");
        problemDetails.Status.Should().Be(400);

        problemDetails.Errors.Should().NotBeNull();

        problemDetails.Errors.Should().ContainKey("Ship.NegativeWidth");
        problemDetails.Errors["Ship.NegativeWidth"].Should().Contain("Ship width must be greater than 0.");

        problemDetails.Errors.Should().ContainKey("Ship.NegativeLength");
        problemDetails.Errors["Ship.NegativeLength"].Should().Contain("Ship length must be greater than 0.");
    }

    [Theory]
    [InlineData("")]
    [InlineData("123456")]
    [InlineData("12345678")]
    [InlineData("1234567890121234567890123456789012345678901234567890123456789012345678901234567890345678901234567890123456789012345678901234567890")]
    public async Task Given_ImoWrongLength_When_RegisteringNewShip_Then_BadRequestWithErrors(string longImoNumber)
    {
        // Arrange
        var request = new RegisterTankerShipRequest(
            ImoNumber: longImoNumber,
            Name: "Titanic",
            Length: 100,
            Width: 50,
            Tanks: [
                new Tank("Diesel", 1024.123f),
                new Tank("HeavyFuel", 10),
                new Tank("Diesel", 10000),
                new Tank("HeavyFuel", 124.5f),
            ]
        );

        // Act
        var response = await _client.PostAsJsonAsync(_baseUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsWithErrors>();

        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Be("One or more validation errors occurred.");
        problemDetails.Status.Should().Be(400);

        problemDetails.Errors.Should().NotBeNull();

        problemDetails.Errors.Should().ContainKey("Ship.InvalidImoFormat");
        problemDetails.Errors["Ship.InvalidImoFormat"].Should().Contain("The IMO number must be 7 characters long and contain only digits.");
    }

    [Theory]
    [InlineData("-234567")]
    [InlineData("1a34567")]
    [InlineData("abcdefg")]
    [InlineData("123456\u2605")]
    public async Task Given_ImoWithLetters_When_RegisteringNewShip_Then_BadRequestWithErrors(string lettersImoNumber)
    {
        // Arrange
        var request = new RegisterTankerShipRequest(
            ImoNumber: lettersImoNumber,
            Name: "Titanic",
            Length: 100,
            Width: 50,
            Tanks: [
                new Tank("Diesel", 1024.123f),
                new Tank("HeavyFuel", 10),
                new Tank("Diesel", 10000),
                new Tank("HeavyFuel", 124.5f),
            ]
        );

        // Act
        var response = await _client.PostAsJsonAsync(_baseUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsWithErrors>();

        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Be("One or more validation errors occurred.");
        problemDetails.Status.Should().Be(400);

        problemDetails.Errors.Should().NotBeNull();

        problemDetails.Errors.Should().ContainKey("Ship.InvalidImoFormat");
        problemDetails.Errors["Ship.InvalidImoFormat"].Should().Contain("The IMO number must be 7 characters long and contain only digits.");
    }

    [Theory]
    [InlineData("9074722")] // checksum = 139
    [InlineData("7601233")] // checksum = 101
    [InlineData("1234564")] // checksum = 77
    [InlineData("1000005")] // checksum = 7
    [InlineData("0100005")] // checksum = 6
    [InlineData("0010006")] // checksum = 5
    [InlineData("0001008")] // checksum = 4
    [InlineData("0000102")] // checksum = 3
    [InlineData("0000011")] // checksum = 2
    [InlineData("0000001")] // checksum = 0
    public async Task Given_InvalidImoChecksum_When_RegisteringNewShip_Then_BadRequestWithErrors(string wrongChecksumImoNumber)
    {
        // Arrange
        var request = new RegisterTankerShipRequest(
            ImoNumber: wrongChecksumImoNumber,
            Name: "Titanic",
            Length: 100,
            Width: 20,
            Tanks: [
                new Tank("Diesel", 1024.123f),
                new Tank("HeavyFuel", 10),
                new Tank("Diesel", 10000),
                new Tank("HeavyFuel", 124.5f),
            ]
        );

        // Act
        var response = await _client.PostAsJsonAsync(_baseUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsWithErrors>();

        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Be("One or more validation errors occurred.");
        problemDetails.Status.Should().Be(400);

        problemDetails.Errors.Should().NotBeNull();
        problemDetails.Errors.Should().ContainKey("Ship.InvalidImoChecksum");
        problemDetails.Errors["Ship.InvalidImoChecksum"].Should().Contain("The provided IMO number has invalid checksum.");
    }

    [Theory]
    [InlineData("Gasoline")] 
    [InlineData("fuelOil")] 
    [InlineData("")] 
    public async Task Given_InvalidTankFuelType_When_RegisteringNewShip_Then_BadRequestWithErrors(string wrongFuelType)
    {
        // Arrange
        var request = new RegisterTankerShipRequest(
            ImoNumber: "1234567",
            Name: "Titanic",
            Length: 100,
            Width: 20,
            Tanks: [
                new Tank("Diesel", 1024.123f),
                new Tank("HeavyFuel", 10),
                new Tank(wrongFuelType, 10000),
                new Tank(wrongFuelType, 124.5f),
            ]
        );

        // Act
        var response = await _client.PostAsJsonAsync(_baseUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsWithErrors>();

        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Be("One or more validation errors occurred.");
        problemDetails.Status.Should().Be(400);

        problemDetails.Errors.Should().NotBeNull();
        problemDetails.Errors.Should().ContainKey("Tank.InvalidFuelType");
        problemDetails.Errors["Tank.InvalidFuelType"].Should().Contain("Fuel type must be one of: 'Diesel', 'HeavyFuel'.");
    }

    [Theory]
    [InlineData(0)] 
    [InlineData(-10)] 
    [InlineData(-123.45f)] 
    public async Task Given_InvalidTankCapacity_When_RegisteringNewShip_Then_BadRequestWithErrors(float wrongCapacity)
    {
        // Arrange
        var request = new RegisterTankerShipRequest(
            ImoNumber: "1234567",
            Name: "Titanic",
            Length: 100,
            Width: 20,
            Tanks: [
                new Tank("Diesel", 1024.123f),
                new Tank("HeavyFuel", wrongCapacity),
                new Tank("Diesel", wrongCapacity),
                new Tank("HeavyFuel", 124.5f),
            ]
        );

        // Act
        var response = await _client.PostAsJsonAsync(_baseUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsWithErrors>();

        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Be("One or more validation errors occurred.");
        problemDetails.Status.Should().Be(400);

        problemDetails.Errors.Should().NotBeNull();
        problemDetails.Errors.Should().ContainKey("Tank.NegativeCapacity");
        problemDetails.Errors["Tank.NegativeCapacity"].Should().Contain("Tank capacity must be greater than 0.");
    }
}