using System;
using MaintGames.Bush.Bootstrap;
using MaintGames.Bush.Contracts.Events;
using MaintGames.Bush.Foundation.Events;
using MaintGames.Bush.Foundation.Logging;
using MaintGames.Bush.Foundation.Services;
using MaintGames.Bush.Infrastructure.Messaging;
using MaintGames.Bush.Tests.EditMode.Support;
using NUnit.Framework;

namespace MaintGames.Bush.Tests.EditMode.Bootstrap
{
    public class GameCompositionTests
    {
        [Test]
        public void Build_NullLog_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => GameComposition.Build(null));
        }

        [Test]
        public void Build_RegistersLogAndEventBus()
        {
            var log = new NullLog();

            ServiceRegistry services = GameComposition.Build(log);

            Assert.AreSame(log, services.Get<ILog>());
            Assert.IsInstanceOf<EventBus>(services.Get<IEventBus>());
        }

        [Test]
        public void Build_SealsRegistry()
        {
            ServiceRegistry services = GameComposition.Build(new NullLog());

            Assert.IsTrue(services.IsSealed);
        }

        [Test]
        public void Reporter_AfterEnable_LogsBootstrapEvent()
        {
            var log = new RecordingLog();
            ServiceRegistry services = GameComposition.Build(log);
            var reporter = new BootstrapReporter(services.Get<IEventBus>(), log);
            reporter.Enable();

            services.Get<IEventBus>().Publish(new GameBootstrapped(2, 1.5));

            Assert.AreEqual(1, log.Entries.Count);
            Assert.AreEqual(LogLevel.Info, log.Entries[0].Level);
            StringAssert.Contains("2 services", log.Entries[0].Message);
            StringAssert.Contains("1.50 s", log.Entries[0].Message);
        }

        [Test]
        public void Reporter_AfterDisable_LogsNothing()
        {
            var log = new RecordingLog();
            ServiceRegistry services = GameComposition.Build(log);
            var reporter = new BootstrapReporter(services.Get<IEventBus>(), log);
            reporter.Enable();
            reporter.Disable();

            services.Get<IEventBus>().Publish(new GameBootstrapped(2, 1.5));

            Assert.AreEqual(0, log.Entries.Count);
        }
    }
}
