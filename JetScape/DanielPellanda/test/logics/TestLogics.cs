using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using JetScape.game.logics;
using JetScape.game.utility;

namespace JetScape.test.logics
{
    [TestFixture]
    public class TestLogics
    {
        private ILogics log;

        [SetUp]
        public void Setup()
        {
            log = new Logics();
        }

        [Test]
        public void Test()
        {
            Assert.That(log.GameState, Is.EqualTo(GameState.MENU));
            Assert.That(log.Entities[EntityType.PLAYER].Count, Is.EqualTo(0));

            log.GameState = GameState.INGAME;

            Assert.That(log.GameState, Is.EqualTo(GameState.INGAME));
            Assert.That(log.Entities[EntityType.PLAYER].Count, Is.EqualTo(1));

            log.GameState = GameState.ENDGAME;

            Assert.That(log.GameState, Is.EqualTo(GameState.ENDGAME));
            Assert.That(log.Entities[EntityType.PLAYER].Count, Is.EqualTo(1));

            log.GameState = GameState.MENU;

            Assert.That(log.GameState, Is.EqualTo(GameState.MENU));
            Assert.That(log.Entities[EntityType.PLAYER].Count, Is.EqualTo(0));

            log.GameState = GameState.PAUSED;

            Assert.That(log.GameState, Is.Not.EqualTo(GameState.PAUSED));
            Assert.That(log.Entities[EntityType.PLAYER].Count, Is.EqualTo(0));

            log.GameState = GameState.INGAME;
            log.GameState = GameState.PAUSED;

            Assert.That(log.GameState, Is.EqualTo(GameState.PAUSED));
            Assert.That(log.Entities[EntityType.PLAYER].Count, Is.EqualTo(1));

        }
    }
}
