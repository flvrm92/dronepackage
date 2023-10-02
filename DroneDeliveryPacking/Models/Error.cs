
namespace DroneDeliveryPacking.Models;

public record Error
{
  public List<string> Messages { get; } = new();

  public void AddMessage(string message) => Messages.Add(message);
}
