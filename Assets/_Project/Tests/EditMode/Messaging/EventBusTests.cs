using System;
using System.Collections.Generic;
using MaintGames.Bush.Foundation.Logging;
using MaintGames.Bush.Infrastructure.Messaging;
using MaintGames.Bush.Tests.EditMode.Support;
using NUnit.Framework;

namespace MaintGames.Bush.Tests.EditMode.Messaging
{
    public class EventBusTests
    {
        private RecordingLog _log;
        private EventBus _bus;

        [SetUp]
        public void SetUp()
        {
            _log = new RecordingLog();
            _bus = new EventBus(_log);
        }

        [Test]
        public void Constructor_NullLog_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new EventBus(null));
        }

        [Test]
        public void Subscribe_NullHandler_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => _bus.Subscribe<SomethingHappened>(null));
        }

        [Test]
        public void Unsubscribe_NullHandler_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => _bus.Unsubscribe<SomethingHappened>(null));
        }

        [Test]
        public void Publish_WithoutSubscribers_DoesNothing()
        {
            _bus.Publish(new SomethingHappened(1));

            Assert.AreEqual(0, _log.Entries.Count);
        }

        [Test]
        public void Publish_WithSubscriber_DeliversEvent()
        {
            int received = -1;
            _bus.Subscribe<SomethingHappened>(e => received = e.Value);

            _bus.Publish(new SomethingHappened(42));

            Assert.AreEqual(42, received);
        }

        [Test]
        public void Publish_MultipleSubscribers_DeliversInSubscriptionOrder()
        {
            var order = new List<int>();
            _bus.Subscribe<SomethingHappened>(e => order.Add(1));
            _bus.Subscribe<SomethingHappened>(e => order.Add(2));
            _bus.Subscribe<SomethingHappened>(e => order.Add(3));

            _bus.Publish(new SomethingHappened(0));

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, order);
        }

        [Test]
        public void Publish_OtherEventType_DoesNotReachSubscriber()
        {
            int calls = 0;
            _bus.Subscribe<SomethingHappened>(e => calls++);

            _bus.Publish(new SomethingElseHappened());

            Assert.AreEqual(0, calls);
        }

        [Test]
        public void Subscribe_SameHandlerTwice_DeliversOnce()
        {
            int calls = 0;
            Action<SomethingHappened> handler = e => calls++;
            _bus.Subscribe(handler);
            _bus.Subscribe(handler);

            _bus.Publish(new SomethingHappened(0));

            Assert.AreEqual(1, calls);
        }

        [Test]
        public void Unsubscribe_AfterSubscribe_StopsDelivery()
        {
            int calls = 0;
            Action<SomethingHappened> handler = e => calls++;
            _bus.Subscribe(handler);
            _bus.Unsubscribe(handler);

            _bus.Publish(new SomethingHappened(0));

            Assert.AreEqual(0, calls);
        }

        [Test]
        public void Unsubscribe_EqualDelegateInstance_StopsDelivery()
        {
            var target = new Counter();
            _bus.Subscribe<SomethingHappened>(target.OnSomething);
            _bus.Unsubscribe<SomethingHappened>(target.OnSomething);

            _bus.Publish(new SomethingHappened(0));

            Assert.AreEqual(0, target.Calls);
        }

        [Test]
        public void Unsubscribe_UnknownHandler_IsIgnored()
        {
            int calls = 0;
            _bus.Subscribe<SomethingHappened>(e => calls++);

            _bus.Unsubscribe<SomethingHappened>(e => { });
            _bus.Unsubscribe<SomethingElseHappened>(e => { });
            _bus.Publish(new SomethingHappened(0));

            Assert.AreEqual(1, calls);
        }

        [Test]
        public void Unsubscribe_MiddleHandler_KeepsOthersInOrder()
        {
            var order = new List<int>();
            Action<SomethingHappened> first = e => order.Add(1);
            Action<SomethingHappened> second = e => order.Add(2);
            Action<SomethingHappened> third = e => order.Add(3);
            _bus.Subscribe(first);
            _bus.Subscribe(second);
            _bus.Subscribe(third);

            _bus.Unsubscribe(second);
            _bus.Publish(new SomethingHappened(0));

            CollectionAssert.AreEqual(new[] { 1, 3 }, order);
        }

        [Test]
        public void Publish_HandlerUnsubscribesLaterHandler_SkipsRemovedHandler()
        {
            var order = new List<int>();
            Action<SomethingHappened> second = e => order.Add(2);
            _bus.Subscribe<SomethingHappened>(e =>
            {
                order.Add(1);
                _bus.Unsubscribe(second);
            });
            _bus.Subscribe(second);

            _bus.Publish(new SomethingHappened(0));

            CollectionAssert.AreEqual(new[] { 1 }, order);
        }

        [Test]
        public void Publish_HandlerUnsubscribesItself_StillRunsOtherHandlers()
        {
            var order = new List<int>();
            Action<SomethingHappened> first = null;
            first = e =>
            {
                order.Add(1);
                _bus.Unsubscribe(first);
            };
            _bus.Subscribe(first);
            _bus.Subscribe<SomethingHappened>(e => order.Add(2));

            _bus.Publish(new SomethingHappened(0));
            _bus.Publish(new SomethingHappened(0));

            CollectionAssert.AreEqual(new[] { 1, 2, 2 }, order);
        }

        [Test]
        public void Publish_HandlerSubscribesNewHandler_NewHandlerMissesCurrentEvent()
        {
            int lateCalls = 0;
            bool subscribed = false;
            _bus.Subscribe<SomethingHappened>(e =>
            {
                if (!subscribed)
                {
                    subscribed = true;
                    _bus.Subscribe<SomethingHappened>(x => lateCalls++);
                }
            });

            _bus.Publish(new SomethingHappened(0));
            Assert.AreEqual(0, lateCalls);

            _bus.Publish(new SomethingHappened(0));
            Assert.AreEqual(1, lateCalls);
        }

        [Test]
        public void Publish_HandlerThrows_LogsErrorAndStillRunsOtherHandlers()
        {
            int calls = 0;
            _bus.Subscribe<SomethingHappened>(e => throw new InvalidOperationException("boom"));
            _bus.Subscribe<SomethingHappened>(e => calls++);

            _bus.Publish(new SomethingHappened(0));

            Assert.AreEqual(1, calls);
            Assert.AreEqual(1, _log.Count(LogLevel.Error));
            StringAssert.Contains("boom", _log.Entries[0].Message);
        }

        [Test]
        public void Publish_HandlerPublishesOtherEvent_IsDeliveredNested()
        {
            int nestedValue = -1;
            _bus.Subscribe<SomethingElseHappened>(e => nestedValue = 7);
            _bus.Subscribe<SomethingHappened>(e => _bus.Publish(new SomethingElseHappened()));

            _bus.Publish(new SomethingHappened(0));

            Assert.AreEqual(7, nestedValue);
        }

        [Test]
        public void Publish_EndlessRecursion_StopsAtDepthLimitAndLogsError()
        {
            int calls = 0;
            _bus.Subscribe<SomethingHappened>(e =>
            {
                calls++;
                _bus.Publish(new SomethingHappened(0));
            });

            _bus.Publish(new SomethingHappened(0));

            Assert.AreEqual(EventBus.MaxPublishDepth, calls);
            Assert.AreEqual(1, _log.Count(LogLevel.Error));
        }

        [Test]
        public void Publish_AfterDepthLimitHit_WorksAgain()
        {
            int calls = 0;
            Action<SomethingHappened> loop = null;
            loop = e => _bus.Publish(new SomethingHappened(0));
            _bus.Subscribe(loop);
            _bus.Publish(new SomethingHappened(0));
            _bus.Unsubscribe(loop);

            _bus.Subscribe<SomethingHappened>(e => calls++);
            _bus.Publish(new SomethingHappened(0));

            Assert.AreEqual(1, calls);
        }

        private sealed class Counter
        {
            public int Calls;

            public void OnSomething(SomethingHappened evt)
            {
                Calls++;
            }
        }
    }
}
