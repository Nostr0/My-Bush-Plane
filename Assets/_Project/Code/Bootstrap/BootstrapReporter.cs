using System.Globalization;
using MaintGames.Bush.Contracts.Events;
using MaintGames.Bush.Foundation.Events;
using MaintGames.Bush.Foundation.Logging;

namespace MaintGames.Bush.Bootstrap
{
    /// <summary>
    /// Example subscriber: logs when <see cref="GameBootstrapped"/> arrives. Shows the Enable/Disable pairing
    /// every subscriber uses. Delete it once real subscribers exist.
    /// </summary>
    public sealed class BootstrapReporter
    {
        private const string LogCategory = "Bootstrap";

        private readonly IEventBus _events;
        private readonly ILog _log;

        public BootstrapReporter(IEventBus events, ILog log)
        {
            _events = events;
            _log = log;
        }

        public void Enable()
        {
            _events.Subscribe<GameBootstrapped>(OnGameBootstrapped);
        }

        public void Disable()
        {
            _events.Unsubscribe<GameBootstrapped>(OnGameBootstrapped);
        }

        private void OnGameBootstrapped(GameBootstrapped evt)
        {
            string seconds = evt.StartupRealSeconds.ToString("F2", CultureInfo.InvariantCulture);
            _log.Info(LogCategory, "Bootstrap complete: " + evt.ServiceCount + " services, " + seconds
                + " s since startup.");
        }
    }
}
