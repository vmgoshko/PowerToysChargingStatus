using ChargingStatus.BatteryInfo;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Windows.Devices.Power;
using Windows.System.Power;

namespace ChargingStatus.Pages;

internal sealed partial class ChargingStatusDockItem : ListItem, IDisposable
{
    private static readonly TimeSpan FallbackRefreshInterval = TimeSpan.FromSeconds(2);

    private readonly Timer _timer;
    private readonly ChargingStatusPage _page;
    private readonly Battery _battery = Battery.AggregateBattery;
    private readonly Lock _syncRoot = new();
    private BatterySnapshot? _lastSnapshot;
    private bool _disposed;

    public ChargingStatusDockItem(ChargingStatusPage page)
        : base(page)
    {
        _page = page;
        Title = "Battery";
        Subtitle = "Loading…";
        Icon = IconHelpers.FromRelativePath(BatteryIconPaths.Neutral);

        _battery.ReportUpdated += OnBatteryReportUpdated;
        PowerManager.RemainingChargePercentChanged += OnPowerStateChanged;
        PowerManager.BatteryStatusChanged += OnPowerStateChanged;
        PowerManager.PowerSupplyStatusChanged += OnPowerStateChanged;

        Refresh();
        _timer = new Timer(_ => Refresh(), null, FallbackRefreshInterval, FallbackRefreshInterval);
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _battery.ReportUpdated -= OnBatteryReportUpdated;
        PowerManager.RemainingChargePercentChanged -= OnPowerStateChanged;
        PowerManager.BatteryStatusChanged -= OnPowerStateChanged;
        PowerManager.PowerSupplyStatusChanged -= OnPowerStateChanged;
        _timer.Dispose();
        GC.SuppressFinalize(this);
    }

    private void OnBatteryReportUpdated(Battery sender, object args) => Refresh();

    private void OnPowerStateChanged(object? sender, object args) => Refresh();

    private void Refresh()
    {
        lock (_syncRoot)
        {
            if (_disposed)
            {
                return;
            }

            BatterySnapshot snapshot = BatterySnapshotProvider.Read();
            if (snapshot == _lastSnapshot)
            {
                return;
            }

            _lastSnapshot = snapshot;

            Title = snapshot.Percent is int percent ? $"{percent}%" : "Battery";
            Subtitle = snapshot.ChargeRateMilliwatts switch
            {
                > 0 => $"Charging • {snapshot.RateText}",
                < 0 => $"Discharging • {snapshot.RateText}",
                _ => snapshot.StatusText,
            };
            Icon = IconHelpers.FromRelativePath(BatteryIconPaths.For(snapshot));

            _page.UpdateSnapshot(snapshot);
        }
    }
}
