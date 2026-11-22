namespace MaintGames.Bush.Foundation.Logging
{
    /// <summary>Convenience overloads for <see cref="ILog"/>.</summary>
    public static class LogExtensions
    {
        public static void Info(this ILog log, string category, string message)
        {
            log.Write(LogLevel.Info, category, message);
        }

        public static void Warning(this ILog log, string category, string message)
        {
            log.Write(LogLevel.Warning, category, message);
        }

        public static void Error(this ILog log, string category, string message)
        {
            log.Write(LogLevel.Error, category, message);
        }
    }
}
