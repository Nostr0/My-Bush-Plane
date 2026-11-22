namespace MaintGames.Bush.Foundation.Logging
{
    /// <summary>Discards all entries. For tests and for code that is allowed to run without output.</summary>
    public sealed class NullLog : ILog
    {
        public void Write(LogLevel level, string category, string message)
        {
        }
    }
}
