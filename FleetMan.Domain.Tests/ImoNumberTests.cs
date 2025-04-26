using FleetMan.Domain.Entities;
using Xunit;

namespace FleetMan.Domain.Tests;

public class ImoNumberTests
{
    [Theory]
    [InlineData("9074729")] // checksum = 139
    [InlineData("7601231")] // checksum = 101
    [InlineData("1234567")] // checksum = 77
    [InlineData("1000007")] // checksum = 7
    [InlineData("0100006")] // checksum = 6
    [InlineData("0010005")] // checksum = 5
    [InlineData("0001004")] // checksum = 4
    [InlineData("0000103")] // checksum = 3
    [InlineData("0000012")] // checksum = 2
    [InlineData("0000000")] // checksum = 0
    public void Given_ValidImo_When_CallingHasValidChecksum_Then_ReturnsTrue(string imo)
    {
        var result = ImoNumber.HasValidChecksum(imo);
        Assert.True(result);
    }

    [Theory]
    [InlineData("9074721")] // checksum = 139
    [InlineData("7601232")] // checksum = 101
    [InlineData("1234563")] // checksum = 77
    [InlineData("1000004")] // checksum = 7
    [InlineData("0100005")] // checksum = 6
    [InlineData("0010006")] // checksum = 5
    [InlineData("0001007")] // checksum = 4
    [InlineData("0000108")] // checksum = 3
    [InlineData("0000019")] // checksum = 2
    [InlineData("0000001")] // checksum = 0
    public void Given_InvalidImo_When_CallingHasValidChecksum_Then_ReturnsFalse(string imo)
    {
        var result = ImoNumber.HasValidChecksum(imo);
        Assert.False(result);
    }

    [Theory]
    [InlineData("9074729")] // checksum = 139
    [InlineData("7601231")] // checksum = 101
    [InlineData("1234567")] // checksum = 77
    [InlineData("1000007")] // checksum = 7
    [InlineData("0100006")] // checksum = 6
    [InlineData("0010005")] // checksum = 5
    [InlineData("0001004")] // checksum = 4
    [InlineData("0000103")] // checksum = 3
    [InlineData("0000012")] // checksum = 2
    [InlineData("0000000")] // checksum = 0
    public void Given_ValidImo_When_CallingCreate_Then_NoError(string imo)
    {
        var result = ImoNumber.Create(imo);

        Assert.False(result.IsError);
        Assert.Equal(imo, result.Value.Value);
    }

    [Theory]
    [InlineData("123456")]  // too short
    [InlineData("12345678")]// too long
    [InlineData("12ab567")] // letters
    public void Given_InvalidFormat_When_CallingCreate_Then_InvalidImoFormatError(string imo)
    {
        var result = ImoNumber.Create(imo);

        Assert.True(result.IsError);
        Assert.Equal(Errors.Errors.ImoNumber.InvalidFormat, result.FirstError);
    }


    [Theory]
    [InlineData("9074721")] // checksum = 139
    [InlineData("7601232")] // checksum = 101
    [InlineData("1234563")] // checksum = 77
    [InlineData("1000004")] // checksum = 7
    [InlineData("0100005")] // checksum = 6
    [InlineData("0010006")] // checksum = 5
    [InlineData("0001007")] // checksum = 4
    [InlineData("0000108")] // checksum = 3
    [InlineData("0000019")] // checksum = 2
    [InlineData("0000001")] // checksum = 0
    public void Given_InvalidCheckDigit_When_CallingCreate_Then_InvalidImoChecksumError(string imo)
    {
        var result = ImoNumber.Create(imo);

        Assert.True(result.IsError);
        Assert.Equal(Errors.Errors.ImoNumber.InvalidChecksum, result.FirstError);
    }
}
