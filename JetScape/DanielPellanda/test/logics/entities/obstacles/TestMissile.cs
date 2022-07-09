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
using JetScape.game.logics.entities.obstacles;
using JetScape.game.logics.entities.obstacles.missile;

namespace JetScape.test.logics.entities.obstacles
{
    [TestFixture]
    public class TestMissile
    {
        private ILogics log;
        private Point pos;
        private SpeedHandler movement;
        private IObstacle missile;
        private IPlayer player;

        [SetUp]
        public void Setup()
        {
            log = new Logics();
            pos = new Point(35 * GameWindow.ScreenInfo.TileSize, GameWindow.ScreenInfo.TileSize);
            player = new Player(log);
            movement = new SpeedHandler(500.0, 10.0, 5000.0);
            missile = new Missile(log, pos, player, movement);
        }

        [Test]
        public void Test()
        {
            Assert.True(missile.EntityType == EntityType.MISSILE);
            log.Entities[EntityType.MISSILE].Add(missile);

            Assert.False(missile.IsOnScreenBounds);
            Assert.False(missile.IsOnClearArea);
            Assert.True(missile.IsOnSpawnArea);

            while (missile.Position.X >= GameWindow.ScreenInfo.Width)
            {
                missile.Update();
            }
            missile.Update();

            Assert.True(missile.IsOnScreenBounds);
            Assert.False(missile.IsOnClearArea);
            Assert.False(missile.IsOnSpawnArea);

            while (missile.Position.X >= -GameWindow.ScreenInfo.TileSize)
            {
                missile.Update();
            }
            missile.Update();

            Assert.False(missile.IsOnScreenBounds);
            Assert.True(missile.IsOnClearArea);
            Assert.False(missile.IsOnSpawnArea);

            missile.Clean();
            missile.Update();

            Assert.False(missile.IsOnScreenBounds);
            Assert.False(missile.IsOnClearArea);
            Assert.True(missile.IsOnSpawnArea);
            Assert.True(log.Entities[EntityType.MISSILE].Count == 0);
        }
    }
}
