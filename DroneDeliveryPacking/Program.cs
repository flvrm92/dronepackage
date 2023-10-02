using System.Diagnostics;
using DroneDeliveryPacking;
using DroneDeliveryPacking.Models;
using DroneDeliveryPacking.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = new ConfigurationBuilder();
BuildConfig(builder);

var inputPath = builder.Build().GetSection("InputFile");

if (!File.Exists(inputPath.Value))
{
  Console.WriteLine("Input file not found. Please check the README.md to configure the project properly.");
  return;
}

var host = Host.CreateDefaultBuilder(args).ConfigureServices((context, services) =>
{
  services.AddTransient<InputService>();
  services.AddTransient<DeliveryService>();
}).Build();

var sw = new Stopwatch();
sw.Start();
Console.WriteLine("Starting process...");

var inputService = ActivatorUtilities.CreateInstance<InputService>(host.Services);
var deliveryService = ActivatorUtilities.CreateInstance<DeliveryService>(host.Services);

var file = await File.ReadAllTextAsync(inputPath.Value);

var error = inputService.Validate(file);
if (error.Messages.Any())
{
  error.Messages.ForEach(Console.WriteLine);
  return;
}

var instructions = file.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

var drones = GetDrones(instructions);

var routes = GetRoutes(instructions, drones);

var trips = deliveryService.Start(drones, routes);

var tripsGrouped = trips.GroupBy(x => x.Drone);

foreach (var trip in tripsGrouped)
{
  Console.WriteLine(trip.Key);
  var tripsByDrone = trips.Where(delivery => delivery.Drone == trip.Key).ToList();
  
  for (var i = 0; i < tripsByDrone.Count; i++)
  {
    Console.WriteLine($"Trip #{i+1}");
    Console.WriteLine($"{tripsByDrone[i]}");
  }
  Console.WriteLine();
}

sw.Stop();

Console.WriteLine($"Deliveries completed in {sw.ElapsedMilliseconds}ms");

static List<Drone> GetDrones(string[] instructions)
{
  var droneInstructions = instructions.FirstOrDefault()!;
  var droneSettings = droneInstructions.Split(",");
  
  var drones = new List<Drone>();

  for (var index = 0; index < droneSettings.Length; index++)
  {
    if (Utility.ShouldSkip(index)) continue;
    drones.Add(new Drone(droneSettings[index].Trim(), droneSettings[index + 1].Trim()));
  }

  return drones;
}

static List<Route> GetRoutes(string[] instructions, IReadOnlyCollection<Drone> drones)
{
  var routeInstructions = instructions.Skip(1).ToArray();
  var totalRoutes = routeInstructions
    .Select(route => new Route(route.Split(",")[0].Trim(), route.Split(",")[1].Trim()))
    .ToList();

  var routesToBeDeliveried =
    totalRoutes.Where(route => route.Weight <= drones.MaxBy(drone => drone.MaximumWeight)!.MaximumWeight)
      .ToList();

  return routesToBeDeliveried;
}

static void BuildConfig(IConfigurationBuilder builder)
{
  builder.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
}
