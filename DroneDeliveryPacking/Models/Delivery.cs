
namespace DroneDeliveryPacking.Models;

public record Delivery(string Drone)
{
  public List<string> Route { get; } = new();

  public void AddRoute(string route) => Route.Add(route);

  public override string ToString() => $"{string.Join(", ", Route)}";
}
