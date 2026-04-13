using System.Runtime.InteropServices;
using Microsoft.CommandPalette.Extensions;

namespace ChargingStatus;

[Guid("96f48378-b9f8-4ea9-91a1-0dbf4f3f3f20")]
public sealed partial class ChargingStatusExtension : IExtension
{
    private readonly ManualResetEvent _extensionDisposedEvent;
    private readonly ChargingStatusCommandsProvider _provider = new();

    public event EventHandler<ManualResetEvent>? Release;

    public ChargingStatusExtension(ManualResetEvent extensionDisposedEvent)
    {
        _extensionDisposedEvent = extensionDisposedEvent;
    }

    public object GetProvider(ProviderType providerType) =>
        providerType == ProviderType.Commands ? _provider : null!;

    public void Dispose() => Release?.Invoke(this, _extensionDisposedEvent);
}
