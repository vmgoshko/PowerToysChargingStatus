using Windows.Devices.Power;
using Windows.System.Power;

namespace ChargingStatus.BatteryInfo;

internal static class BatterySnapshotProvider
{
    public static BatterySnapshot Read()
    {
        BatteryReport report = Battery.AggregateBattery.GetReport();

        return new BatterySnapshot(
            Percent: PowerManager.RemainingChargePercent,
            ChargeRateMilliwatts: ToInt(report.ChargeRateInMilliwatts),
            RemainingCapacityMilliwattHours: ToInt(report.RemainingCapacityInMilliwattHours),
            FullChargeCapacityMilliwattHours: ToInt(report.FullChargeCapacityInMilliwattHours),
            Status: report.Status);
    }

    private static int? ToInt(int? value) => value;
}
