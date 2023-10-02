namespace DroneDeliveryPacking.Tests;

public class UtilityTests
{
  [Theory]
  [InlineData("100", 100)]
  [InlineData("[200]", 200)]
  [InlineData("[500]", 500)]
  [InlineData("[0]", 0)]
  [InlineData("[-10]", -10)]
  public void SanitizeMaximumWeight_ValidInput_ReturnsExpectedResult(string input, int expected)
  {
    // Act
    var result = Utility.SanitizeMaximumWeight(input);

    // Assert
    Assert.Equal(expected, result);
  }
}
