using MaintGames.Bush.Foundation.Logging;
using UnityEngine;

namespace MaintGames.Bush.Infrastructure.Logging
{
    /// <summary>
    /// <see cref="ILog"/> that writes to the Unity console. This is the one place where <c>Debug.Log</c> is
    /// allowed, because everything else goes through <see cref="ILog"/>.
    /// </summary>
    public sealed class UnityLog : ILog
    {
        public void Write(LogLevel level, string category, string message)
        {
            string line = "[" + category + "] " + message;
            switch (level)
            {
                case LogLevel.Warning:
                    Debug.LogWarning(line);
                    break;
                case LogLevel.Error:
                    Debug.LogError(line);
                    break;
                default:
                    Debug.Log(line);
                    break;
            }
        }
    }
}
