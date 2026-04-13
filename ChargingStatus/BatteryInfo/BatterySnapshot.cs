using Windows.Devices.Power;
using Windows.System.Power;

namespace ChargingStatus.BatteryInfo;

internal sealed record BatterySnapshot(
    int? Percent,
    int? ChargeRateMilliwatts,
    int? RemainingCapacityMilliwattHours,
    int? FullChargeCapacityMilliwattHours,
    BatteryStatus Status)
{
    public bool HasBattery => Percent is not null || RemainingCapacityMilliwattHours is not null;

    public string StatusText =>
        Status switch
        {
            BatteryStatus.Charging => "Charging",
            BatteryStatus.Discharging => "Discharging",
            BatteryStatus.Idle => "Plugged in",
            BatteryStatus.NotPresent => "No battery",
            _ => "Unknown",
        };

    public string PercentText => Percent is int value ? $"{value}%" : "Unknown";

    public string RateText
    {
        get
        {
            if (ChargeRateMilliwatts is not int rate || rate == 0)
            {
                return "Rate unavailable";
            }

            return $"{Math.Abs(rate) / 1000d:0.0} W";
        }
    }

    public string SummaryText
    {
        get
        {
            if (!HasBattery)
            {
                return "No battery detected";
            }

            return ChargeRateMilliwatts switch
            {
                > 0 => $"{PercentText} • Charging • {RateText}",
                < 0 => $"{PercentText} • Discharging • {RateText}",
                _ => $"{PercentText} • {StatusText}",
            };
        }
    }

    public string DetailsText
    {
        get
        {
            List<string> parts =
            [
                $"Status: {StatusText}",
                $"Charge: {PercentText}",
                $"Rate: {RateText}"
            ];

            if (RemainingCapacityMilliwattHours is int remaining)
            {
                parts.Add($"Remaining: {remaining} mWh");
            }

            if (FullChargeCapacityMilliwattHours is int full)
            {
                parts.Add($"Full charge capacity: {full} mWh");
            }

            return string.Join(Environment.NewLine, parts);
        }
    }
}
