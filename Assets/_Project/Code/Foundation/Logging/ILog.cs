namespace MaintGames.Bush.Foundation.Logging
{
    /// <summary>
    /// The only way game code writes log output. Every entry carries a category (e.g. "EventBus").
    /// Use the extension methods in <see cref="LogExtensions"/> for the common levels.
    /// </summary>
    public interface ILog
    {
        void Write(LogLevel level, string category, string message);
    }
}
