using DroneDeliveryPacking.Services;

namespace DroneDeliveryPacking.Tests.Services;

public class InputServiceTests
{
  private readonly InputService _inputService;

  public InputServiceTests()
  {
    _inputService = new InputService();
  }

  [Fact]
  public void Should_Return_No_Errors()
  {
    // Arrange
    const string input = "[DroneA], [200]\r\n[LocationA], [200]";
    
    // Act
    var error = _inputService.Validate(input);

    // Assert
    Assert.False(error.Messages.Any());
  }

  [Fact]
  public void Should_Return_No_Lines_Found()
  {
    // Arrange
    // Act
    var error = _inputService.Validate(string.Empty);

    // Assert
    Assert.True(error.Messages.Any());
    Assert.Equal("No lines found", error.Messages.First());
  }

  [Fact]
  public void Should_Return_AtLeast_One_Route_Required()
  {
    // Arrange
    const string input = "[DroneA], [200]\r\n";
    // Act
    var error = _inputService.Validate(input);

    // Assert
    Assert.True(error.Messages.Any());
    Assert.Equal("At least one route is required", error.Messages.First());
  }

  [Fact]
  public void Should_Return_Invalid_Header()
  {
    // Arrange
    const string input = "[DroneA], [200], [DroneB]\r\n[LocationA], [200]";
    // Act
    var error = _inputService.Validate(input);

    // Assert
    Assert.True(error.Messages.Any());
    Assert.Equal("Header line should contain an even number of values (at least one drone name and weight)", error.Messages.First());
  }

  [Fact]
  public void Should_Return_Invalid_Location()
  {
    // Arrange
    const string input = "[DroneA], [200]\r\n[LocationA], [LocationB]";
    // Act
    var error = _inputService.Validate(input);

    // Assert
    Assert.True(error.Messages.Any());
    Assert.Equal("Route line 1 should contain a valid weight (integer)", error.Messages.First());
  }
}
