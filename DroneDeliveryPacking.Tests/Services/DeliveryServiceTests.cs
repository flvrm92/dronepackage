using DroneDeliveryPacking.Models;
using DroneDeliveryPacking.Services;

namespace DroneDeliveryPacking.Tests.Services;
public class DeliveryServiceTests
{
  private readonly DeliveryService _deliveryService;

  public DeliveryServiceTests()
  {
    _deliveryService = new DeliveryService();
  }

  [Fact]
  public void Should_Delivery_All_Routes()
  {
    // Arrange

    var drones = new List<Drone>
    {
      new("DroneA", "200"),
      new("DroneB", "250"),
      new("DroneC", "100")
    };

    var routes = new List<Route>
    {
      new("LocationA", "200"),
      new("LocationB", "150"),
      new("LocationC", "50"),
      new("LocationD", "150"),
      new("LocationE", "100"),
      new("LocationF", "200"),
      new("LocationG", "50"),
      new("LocationH", "80"),
      new("LocationI", "70"),
      new("LocationJ", "50"),
      new("LocationK", "30"),
      new("LocationL", "20"),
      new("LocationM", "50"),
      new("LocationN", "30"),
      new("LocationO", "20"),
      new("LocationP", "90")
    };
    // Act
    var trips = _deliveryService.Start(drones, routes);
    // Assert
    Assert.Equal(8, trips.Count);
    Assert.Equal(3, trips.Count(x => x.Drone == "DroneA"));
    Assert.Equal(3, trips.Count(x => x.Drone == "DroneB"));
    Assert.Equal(2, trips.Count(x => x.Drone == "DroneC"));
  }
}
