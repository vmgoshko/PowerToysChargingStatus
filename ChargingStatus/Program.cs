namespace ChargingStatus;

using Shmuelie.WinRTServer.CsWinRT;

internal static class Program
{
    [MTAThread]
    private static async Task Main(string[] args)
    {
        if (args is ["-RegisterProcessAsComServer", ..])
        {
            await HandleComServerActivationAsync();
        }
    }

    private static async Task HandleComServerActivationAsync()
    {
        await using global::Shmuelie.WinRTServer.ComServer server = new();
        using ManualResetEvent extensionDisposedEvent = new(false);

        ChargingStatusExtension extension = new(extensionDisposedEvent);
        extension.Release += static (_, disposedEvent) => disposedEvent.Set();

        server.RegisterClass<ChargingStatusExtension, Microsoft.CommandPalette.Extensions.IExtension>(() => extension);
        server.Start();
        extensionDisposedEvent.WaitOne();
        server.Stop();
        server.UnsafeDispose();
    }
}
