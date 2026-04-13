using ChargingStatus.BatteryInfo;
using ChargingStatus.Pages;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace ChargingStatus;

public sealed partial class ChargingStatusCommandsProvider : CommandProvider, IDisposable
{
    internal const string ProviderId = "dev.vmgoshko.powertoys.chargingstatus";
    internal const string MainPageCommandId = ProviderId + ".page";
    internal const string DockBandId = ProviderId + ".dock";

    private readonly ChargingStatusPage _page;
    private readonly ChargingStatusDockItem _dockItem;
    private readonly CommandItem _topLevelCommand;
    private readonly WrappedDockItem _dockBand;

    public ChargingStatusCommandsProvider()
    {
        DisplayName = "Battery Charging Status";
        Id = ProviderId;
        Icon = IconHelpers.FromRelativePath(BatteryIconPaths.Neutral);

        _page = new ChargingStatusPage();
        _topLevelCommand = new CommandItem(_page)
        {
            Title = "Battery Charging Status",
            Subtitle = "Battery status and charging rate",
        };

        _dockItem = new ChargingStatusDockItem(_page);
        _dockBand = new WrappedDockItem(
            [_dockItem],
            DockBandId,
            "Battery Charging Status");
    }

    public override ICommandItem[] TopLevelCommands() => [_topLevelCommand];

    public override ICommandItem[]? GetDockBands() => [_dockBand];

    public override ICommandItem? GetCommandItem(string id) =>
        id switch
        {
            DockBandId => _dockBand,
            MainPageCommandId => _topLevelCommand,
            _ => null,
        };

    public override void Dispose()
    {
        _dockItem.Dispose();
        base.Dispose();
    }
}
