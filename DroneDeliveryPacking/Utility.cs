namespace DroneDeliveryPacking;
public static class Utility
{
  public static int SanitizeMaximumWeight(string maximumWeight) =>
    int.Parse(maximumWeight.Replace("[", "").Replace("]", ""));

  public static bool ShouldSkip(int index) => index % 2 != 0;
}
