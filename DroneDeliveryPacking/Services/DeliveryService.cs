using DroneDeliveryPacking.Models;

namespace DroneDeliveryPacking.Services;

public class DeliveryService
{
  private static bool MissingRoutes(List<Delivery> deliveries, List<Route> routes)
  {
    var deliveriedRoutes = deliveries.SelectMany(delivery => delivery.Route).ToList();
    return routes.Any(route => deliveriedRoutes.All(m => m != route.Location)); ;
  }

  public List<Delivery> Start(List<Drone> drones, List<Route> routes)
  {
    var deliveries = new List<Delivery>();

    while (MissingRoutes(deliveries, routes))
    {
      foreach (var drone in drones)
      {
        var alreadyDelivered = deliveries.SelectMany(delivery => delivery.Route);

        var toDeliveryRoutes = routes
          .Where(route => route.Weight <= drone.MaximumWeight && alreadyDelivered.All(x => x != route.Location))
          .ToList();

        if (!toDeliveryRoutes.Any()) break;

        var remainingCapacity = drone.MaximumWeight;

        var delivery = new Delivery(drone.Name);
        foreach (var toDeliveryRoute in toDeliveryRoutes)
        {
          if (remainingCapacity == 0) break;
          if (remainingCapacity < toDeliveryRoute.Weight) continue;

          delivery.AddRoute(toDeliveryRoute.Location);
          remainingCapacity -= toDeliveryRoute.Weight;
        }

        deliveries.Add(delivery);
      }
    }

    return deliveries;
  }
}
