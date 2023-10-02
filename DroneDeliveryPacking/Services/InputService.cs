using DroneDeliveryPacking.Models;

namespace DroneDeliveryPacking.Services;

public class InputService
{
  public Error Validate(string input)
  {
    var error = new Error();

    var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    var droneInstructions = lines.FirstOrDefault();
    if (droneInstructions is null)
    {
      error.AddMessage("No lines found");
      return error;
    }

    var routes = lines.Skip(1).ToArray();
    if (!routes.Any())
    {
      error.AddMessage("At least one route is required");
      return error;
    }

    var headerErrorMessage = ValidateHeader(droneInstructions);
    if (headerErrorMessage is not null) error.AddMessage(headerErrorMessage);

    var routeErrorMessage = ValidateRoutes(routes);
    routeErrorMessage?.ForEach(error.AddMessage);

    return error;
  }

  private static string? ValidateHeader(string droneInstructions)
  {
    var droneSettings = droneInstructions.Split(",");
    if (droneSettings.Length <= 0) return "Missing the header line";

    return !int.IsEvenInteger(droneSettings.Length) 
      ? "Header line should contain an even number of values (at least one drone name and weight)" 
      : null;
  }

  private static List<string>? ValidateRoutes(IReadOnlyList<string> routes)
  {
    var errors = new List<string>();

    for (var i = 0; i < routes.Count; i++)
    {
      if (Utility.ShouldSkip(i)) continue;

      var route = routes[i].Split(",");
      if (route.Length != 2) errors.Add($"Route line {i + 1} should contain two values (location and weight)");

      try
      {
        _ = Utility.SanitizeMaximumWeight(route[1]);
      }
      catch
      {
        errors.Add($"Route line {i + 1} should contain a valid weight (integer)");
      }
    }

    return errors.Any() ? errors : null;
  }
}
