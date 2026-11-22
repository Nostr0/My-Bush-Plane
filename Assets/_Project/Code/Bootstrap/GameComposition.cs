using System;
using MaintGames.Bush.Foundation.Events;
using MaintGames.Bush.Foundation.Logging;
using MaintGames.Bush.Foundation.Services;
using MaintGames.Bush.Infrastructure.Messaging;

namespace MaintGames.Bush.Bootstrap
{
    /// <summary>
    /// Pure C# part of the composition root: creates the services and returns a sealed registry.
    /// Every new service gets one line here, in dependency order.
    /// </summary>
    public static class GameComposition
    {
        public static ServiceRegistry Build(ILog log)
        {
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            var services = new ServiceRegistry();
            services.Register<ILog>(log);
            services.Register<IEventBus>(new EventBus(log));
            services.Seal();
            return services;
        }
    }
}
