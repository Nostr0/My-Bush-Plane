namespace MaintGames.Bush.Contracts.Events
{
    /// <summary>
    /// Published once by the composition root after all services are registered and the registry is sealed.
    /// Example event for the pattern in docs/ARCHITECTURE.md, section 7.2.
    /// </summary>
    public readonly struct GameBootstrapped
    {
        public GameBootstrapped(int serviceCount, double startupRealSeconds)
        {
            ServiceCount = serviceCount;
            StartupRealSeconds = startupRealSeconds;
        }

        /// <summary>Number of registered services.</summary>
        public int ServiceCount { get; }

        /// <summary>Real time since the player started, in seconds (not game time).</summary>
        public double StartupRealSeconds { get; }
    }
}
