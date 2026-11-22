using System;
using MaintGames.Bush.Foundation.Services;
using NUnit.Framework;

namespace MaintGames.Bush.Tests.EditMode.Services
{
    public class ServiceRegistryTests
    {
        private interface IExample
        {
        }

        private interface IOther
        {
        }

        private sealed class Example : IExample
        {
        }

        private sealed class Other : IOther
        {
        }

        [Test]
        public void Get_RegisteredService_ReturnsSameInstance()
        {
            var registry = new ServiceRegistry();
            var example = new Example();
            registry.Register<IExample>(example);

            Assert.AreSame(example, registry.Get<IExample>());
        }

        [Test]
        public void Get_MissingService_Throws()
        {
            var registry = new ServiceRegistry();

            Assert.Throws<InvalidOperationException>(() => registry.Get<IExample>());
        }

        [Test]
        public void TryGet_MissingService_ReturnsFalse()
        {
            var registry = new ServiceRegistry();

            bool found = registry.TryGet(out IExample service);

            Assert.IsFalse(found);
            Assert.IsNull(service);
        }

        [Test]
        public void TryGet_RegisteredService_ReturnsTrue()
        {
            var registry = new ServiceRegistry();
            var example = new Example();
            registry.Register<IExample>(example);

            bool found = registry.TryGet(out IExample service);

            Assert.IsTrue(found);
            Assert.AreSame(example, service);
        }

        [Test]
        public void Register_SameTypeTwice_Throws()
        {
            var registry = new ServiceRegistry();
            registry.Register<IExample>(new Example());

            Assert.Throws<InvalidOperationException>(() => registry.Register<IExample>(new Example()));
        }

        [Test]
        public void Register_Null_Throws()
        {
            var registry = new ServiceRegistry();

            Assert.Throws<ArgumentNullException>(() => registry.Register<IExample>(null));
        }

        [Test]
        public void Register_AfterSeal_Throws()
        {
            var registry = new ServiceRegistry();
            registry.Seal();

            Assert.Throws<InvalidOperationException>(() => registry.Register<IExample>(new Example()));
        }

        [Test]
        public void Get_AfterSeal_StillWorks()
        {
            var registry = new ServiceRegistry();
            var example = new Example();
            registry.Register<IExample>(example);
            registry.Seal();

            Assert.IsTrue(registry.IsSealed);
            Assert.AreSame(example, registry.Get<IExample>());
        }

        [Test]
        public void Count_CountsRegisteredServices()
        {
            var registry = new ServiceRegistry();
            registry.Register<IExample>(new Example());
            registry.Register<IOther>(new Other());

            Assert.AreEqual(2, registry.Count);
        }
    }
}
