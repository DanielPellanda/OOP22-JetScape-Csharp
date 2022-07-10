using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using NUnit.Framework;

using JetScape.game.logics;
using JetScape.game.frame;
using JetScape.game.utility;
using JetScape.game.logics.interactions;
using JetScape.game.logics.entities.player;
using JetScape.game.logics.entities.pickups;
using JetScape.game.logics.entities.pickups.shield;

namespace JetScape.test.logics.entities.pickups
{
    [TestFixture]
    public class TestShield
    {
        private ILogics log;
        private Point pos;
        private SpeedHandler movement;
        private IPickup shield;
        private IPlayer player;

        [SetUp]
        public void Setup()
        {
            log = new Logics();
            pos = new Point(18 * GameWindow.ScreenInfo.TileSize, GameWindow.ScreenInfo.TileSize);
            player = new Player(log);
            movement = new SpeedHandler(250.0, 15.0, 0);
            shield = new Shield(log, pos, player, movement);
        }

        [Test]
        public void Test()
        {
            Assert.That(shield.EntityType, Is.EqualTo((EntityType) EntityType.SHIELD));
            log.Entities[EntityType.SHIELD].Add(shield);

            Assert.False(shield.IsOnScreenBounds);
            Assert.False(shield.IsOnClearArea);
            Assert.True(shield.IsOnSpawnArea);

            while (shield.Position.X >= GameWindow.ScreenInfo.Width)
            {
                shield.Update();
            }
            shield.Update();

            Assert.True(shield.IsOnScreenBounds);
            Assert.False(shield.IsOnClearArea);
            Assert.False(shield.IsOnSpawnArea);

            while (shield.Position.X >= -GameWindow.ScreenInfo.TileSize)
            {
                shield.Update();
            }
            shield.Update();

            Assert.False(shield.IsOnScreenBounds);
            Assert.True(shield.IsOnClearArea);
            Assert.False(shield.IsOnSpawnArea);

            shield.Clean();
            shield.Update();

            Assert.False(shield.IsOnScreenBounds);
            Assert.False(shield.IsOnClearArea);
            Assert.True(shield.IsOnSpawnArea);
            Assert.That(log.Entities[EntityType.SHIELD].Count, Is.EqualTo(0));
        }
    }
}
