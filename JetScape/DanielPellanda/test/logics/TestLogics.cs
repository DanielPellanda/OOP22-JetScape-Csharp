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
            Assert.True(log.GameState == GameState.MENU);
            Assert.True(log.Entities[EntityType.PLAYER].Count == 0);

            log.GameState = GameState.INGAME;

            Assert.True(log.GameState == GameState.INGAME);
            Assert.True(log.Entities[EntityType.PLAYER].Count == 1);

            log.GameState = GameState.ENDGAME;

            Assert.True(log.GameState == GameState.ENDGAME);
            Assert.True(log.Entities[EntityType.PLAYER].Count == 1);

            log.GameState = GameState.MENU;

            Assert.True(log.GameState == GameState.MENU);
            Assert.True(log.Entities[EntityType.PLAYER].Count == 0);

            log.GameState = GameState.PAUSED;

            Assert.False(log.GameState == GameState.PAUSED);
            Assert.True(log.Entities[EntityType.PLAYER].Count == 0);

            log.GameState = GameState.INGAME;
            log.GameState = GameState.PAUSED;

            Assert.True(log.GameState == GameState.PAUSED);
            Assert.True(log.Entities[EntityType.PLAYER].Count == 1);

        }
    }
}
