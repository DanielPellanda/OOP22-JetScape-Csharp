using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using JetScape.game.logics;
using JetScape.game.logics.entities.player;
using JetScape.game.utility;

namespace JetScape.test.logics.entities.player
{
    [TestFixture]
    public class TestPlayer
    {
        private ILogics log;
        private IPlayer player;
        private int updates;


        [SetUp]
        public void Setup()
        {
            log = new Logics();
            player = new Player(log);
            updates = 20;
        }

        [Test]
        public void Test()
        {
            Assert.That(player.EntityType, Is.EqualTo((EntityType) EntityType.PLAYER));
            log.Entities[EntityType.PLAYER].Add(player);

            for (int i = 0; i < updates; i++)
            {
                player.Update();
            }

            Assert.That(player.CurrentScore, Is.EqualTo(updates / 2 - 1));
            Assert.False(player.HasDied);
            Assert.True(player.IsOnScreenBounds);
            Assert.False(player.IsOnClearArea);
            Assert.False(player.IsOnSpawnArea);

            int y = player.Position.Y;
            player.SimulateInput = true;
            for (int i = 0; i < updates; i++)
            {
                player.Update();
            }

            if (player.Position.Y == y)
            {
                Assert.Fail("Jump test failed");
            }
            Assert.False(player.HasDied);
            Assert.True(player.IsOnScreenBounds);
            Assert.False(player.IsOnClearArea);
            Assert.False(player.IsOnSpawnArea);

            player.SimulateInput = false;
            player.Clean();
            player.Update();

            Assert.That(player.CurrentScore, Is.EqualTo(0));
            Assert.False(player.HasDied);
            Assert.True(player.IsOnScreenBounds);
            Assert.False(player.IsOnClearArea);
            Assert.False(player.IsOnSpawnArea);

            Assert.That(log.Entities[EntityType.PLAYER].Count, Is.EqualTo(0));
        }
    }
}
