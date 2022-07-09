using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using JetScape.game.logics;
using JetScape.game.logics.generator;

namespace JetScape.test.logics.generator
{
    public class TestGenerator
    {
        private double interval;
        private ILogics logics;
        private IGenerator generator;

        [SetUp]
        public void Setup() 
        {
            interval = 3.3;
            logics = new Logics();
            generator = new Generator(logics.Entities, interval);
        }

        [Test]
        public void Test()
        {
            Assert.False(generator.IsRunning);
            Assert.False(generator.IsWaiting);

            generator.Start();
            Assert.True(generator.IsRunning);
            Assert.True(generator.IsWaiting);

            generator.Resume();
            Assert.True(generator.IsRunning);
            Assert.False(generator.IsWaiting);

            generator.Pause();
            Assert.True(generator.IsRunning);
            Assert.True(generator.IsWaiting);

            generator.Resume();
            Assert.True(generator.IsRunning);
            Assert.False(generator.IsWaiting);

            generator.Stop();
            Assert.True(generator.IsRunning);
            Assert.True(generator.IsWaiting);

            generator.Resume();
            Assert.True(generator.IsRunning);
            Assert.False(generator.IsWaiting);

            generator.Pause();
            Assert.True(generator.IsRunning);
            Assert.True(generator.IsWaiting);

            generator.Terminate();
            Assert.False(generator.IsRunning);
            Assert.False(generator.IsWaiting);
        }
    }
}
