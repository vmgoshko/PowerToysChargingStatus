namespace ChargingStatus.BatteryInfo;

internal static class BatteryIconPaths
{
    internal const string Neutral = "Assets\\BatteryNeutral44.png";
    internal const string Charging = "Assets\\BatteryCharging44.png";
    internal const string Discharging = "Assets\\BatteryDischarging44.png";

    public static string For(BatterySnapshot snapshot) =>
        snapshot.ChargeRateMilliwatts switch
        {
            > 0 => Charging,
            < 0 => Discharging,
            _ => Neutral,
        };
}
