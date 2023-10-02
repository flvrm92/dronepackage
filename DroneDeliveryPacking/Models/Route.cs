namespace DroneDeliveryPacking.Models;

public record Route
{
  public Route(string location, string weight)
  {
    Location = location;
    Weight = Utility.SanitizeMaximumWeight(weight);
  }

  public string Location { get; }
  public int Weight { get; }
}

