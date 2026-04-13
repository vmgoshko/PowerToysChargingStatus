using ChargingStatus.BatteryInfo;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace ChargingStatus.Pages;

internal sealed partial class ChargingStatusPage : ListPage
{
    private readonly List<ListItem> _items = [];
    private BatterySnapshot _snapshot = BatterySnapshotProvider.Read();

    public ChargingStatusPage()
    {
        Id = ChargingStatusCommandsProvider.MainPageCommandId;
        Icon = IconHelpers.FromRelativePath(BatteryIconPaths.For(_snapshot));
        Title = "Battery Charging Status";
        Name = "Open";
        RebuildItems();
    }

    public override IListItem[] GetItems() => _items.ToArray();

    internal void UpdateSnapshot(BatterySnapshot snapshot)
    {
        _snapshot = snapshot;
        Icon = IconHelpers.FromRelativePath(BatteryIconPaths.For(snapshot));
        RebuildItems();
        RaiseItemsChanged();
    }

    private void RebuildItems()
    {
        _items.Clear();

        _items.Add(new ListItem(new NoOpCommand())
        {
            Title = _snapshot.SummaryText,
            Subtitle = _snapshot.StatusText,
            Icon = IconHelpers.FromRelativePath(BatteryIconPaths.For(_snapshot)),
        });

        _items.Add(new ListItem(new NoOpCommand())
        {
            Title = "Battery details",
            Subtitle = _snapshot.DetailsText.Replace(Environment.NewLine, " • "),
        });

        _items.Add(new ListItem(new OpenUrlCommand("ms-settings:batterysaver-settings"))
        {
            Title = "Open battery settings",
            Subtitle = "Launches Windows battery settings",
        });

        _items.Add(new ListItem(new OpenUrlCommand("ms-settings:powersleep"))
        {
            Title = "Open power & sleep settings",
            Subtitle = "Launches Windows power configuration",
        });
    }
}
