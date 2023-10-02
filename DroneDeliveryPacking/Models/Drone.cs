
namespace DroneDeliveryPacking.Models;

public record Drone
{
  public Drone(string name, string maximumWeight)
  {
    Name = name;
    MaximumWeight = Utility.SanitizeMaximumWeight(maximumWeight);
  }

  public string Name { get; }
  public int MaximumWeight { get; }
}
