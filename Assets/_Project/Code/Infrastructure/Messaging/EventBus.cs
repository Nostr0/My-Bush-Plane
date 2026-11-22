using System;
using System.Collections.Generic;
using MaintGames.Bush.Foundation.Events;
using MaintGames.Bush.Foundation.Logging;

namespace MaintGames.Bush.Infrastructure.Messaging
{
    /// <summary>
    /// Default <see cref="IEventBus"/>. Delivery is synchronous and in subscription order.
    /// <list type="bullet">
    /// <item>Handlers are stored copy-on-write, so publishing allocates nothing.</item>
    /// <item>A handler added during delivery does not receive the event being delivered.</item>
    /// <item>A handler removed during delivery is skipped, so destroyed objects are not called.</item>
    /// <item>An exception in one handler is logged and does not stop the others.</item>
    /// <item>Publishing from inside a handler is allowed up to <see cref="MaxPublishDepth"/> levels.</item>
    /// </list>
    /// Not thread-safe: use from the main thread only.
    /// </summary>
    public sealed class EventBus : IEventBus
    {
        /// <summary>Nesting limit that turns an accidental publish loop into a log entry instead of a crash.</summary>
        public const int MaxPublishDepth = 32;

        private const string LogCategory = "EventBus";

        private readonly ILog _log;
        private readonly Dictionary<Type, object> _channels = new Dictionary<Type, object>();
        private int _publishDepth;

        public EventBus(ILog log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public void Publish<TEvent>(in TEvent evt) where TEvent : struct
        {
            if (_publishDepth >= MaxPublishDepth)
            {
                _log.Error(LogCategory, typeof(TEvent).Name + " dropped: publish depth " + MaxPublishDepth
                    + " exceeded (publish loop?).");
                return;
            }

            if (!_channels.TryGetValue(typeof(TEvent), out object found))
            {
                return;
            }

            var channel = (Channel<TEvent>)found;
            Action<TEvent>[] snapshot = channel.Handlers;

            _publishDepth++;
            try
            {
                for (int i = 0; i < snapshot.Length; i++)
                {
                    Action<TEvent> handler = snapshot[i];

                    // Handlers change rarely, so the membership check only runs after a change.
                    if (!ReferenceEquals(snapshot, channel.Handlers) && Array.IndexOf(channel.Handlers, handler) < 0)
                    {
                        continue;
                    }

                    try
                    {
                        handler(evt);
                    }
                    catch (Exception exception)
                    {
                        _log.Error(LogCategory, "Handler for " + typeof(TEvent).Name + " threw: " + exception);
                    }
                }
            }
            finally
            {
                _publishDepth--;
            }
        }

        public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : struct
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            Channel<TEvent> channel = GetOrCreateChannel<TEvent>();
            Action<TEvent>[] current = channel.Handlers;
            if (Array.IndexOf(current, handler) >= 0)
            {
                return;
            }

            var updated = new Action<TEvent>[current.Length + 1];
            Array.Copy(current, updated, current.Length);
            updated[current.Length] = handler;
            channel.Handlers = updated;
        }

        public void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : struct
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            if (!_channels.TryGetValue(typeof(TEvent), out object found))
            {
                return;
            }

            var channel = (Channel<TEvent>)found;
            Action<TEvent>[] current = channel.Handlers;
            int index = Array.IndexOf(current, handler);
            if (index < 0)
            {
                return;
            }

            if (current.Length == 1)
            {
                channel.Handlers = Array.Empty<Action<TEvent>>();
                return;
            }

            var updated = new Action<TEvent>[current.Length - 1];
            Array.Copy(current, 0, updated, 0, index);
            Array.Copy(current, index + 1, updated, index, current.Length - index - 1);
            channel.Handlers = updated;
        }

        private Channel<TEvent> GetOrCreateChannel<TEvent>() where TEvent : struct
        {
            if (_channels.TryGetValue(typeof(TEvent), out object found))
            {
                return (Channel<TEvent>)found;
            }

            var channel = new Channel<TEvent>();
            _channels.Add(typeof(TEvent), channel);
            return channel;
        }

        private sealed class Channel<TEvent> where TEvent : struct
        {
            public Action<TEvent>[] Handlers = Array.Empty<Action<TEvent>>();
        }
    }
}
