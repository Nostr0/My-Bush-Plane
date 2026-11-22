using System;

namespace MaintGames.Bush.Foundation.Events
{
    /// <summary>
    /// Publish/subscribe channel for game events. Events are <c>readonly struct</c> types in past tense
    /// (see docs/ARCHITECTURE.md, section 7.2). Main thread only.
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Delivers <paramref name="evt"/> to all current subscribers. Publish after the state change, never before.
        /// Do not publish per frame; per-frame data is read, not sent.
        /// </summary>
        void Publish<TEvent>(in TEvent evt) where TEvent : struct;

        /// <summary>Registers <paramref name="handler"/>. Subscribing the same handler twice has no effect.</summary>
        void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : struct;

        /// <summary>
        /// Removes <paramref name="handler"/>. Takes effect immediately, also while an event is being delivered.
        /// Unknown handlers are ignored.
        /// </summary>
        void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : struct;
    }
}
