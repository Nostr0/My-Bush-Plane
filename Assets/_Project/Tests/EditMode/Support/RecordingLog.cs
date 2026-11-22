using System.Collections.Generic;
using MaintGames.Bush.Foundation.Logging;

namespace MaintGames.Bush.Tests.EditMode.Support
{
    /// <summary>Keeps all entries so tests can assert on them.</summary>
    public sealed class RecordingLog : ILog
    {
        public readonly List<Entry> Entries = new List<Entry>();

        public void Write(LogLevel level, string category, string message)
        {
            Entries.Add(new Entry(level, category, message));
        }

        public int Count(LogLevel level)
        {
            int count = 0;
            for (int i = 0; i < Entries.Count; i++)
            {
                if (Entries[i].Level == level)
                {
                    count++;
                }
            }

            return count;
        }

        public readonly struct Entry
        {
            public Entry(LogLevel level, string category, string message)
            {
                Level = level;
                Category = category;
                Message = message;
            }

            public LogLevel Level { get; }

            public string Category { get; }

            public string Message { get; }
        }
    }
}
