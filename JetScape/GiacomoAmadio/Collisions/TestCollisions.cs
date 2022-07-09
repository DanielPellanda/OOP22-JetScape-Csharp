using NUnit.Framework;
using System.Collections.Generic;
using JetScape.game.logics;
using JetScape.game.logics.entities;
using JetScape.game.logics.entities.player;
using JetScape.game.logics.entities.obstacles.missile;
using JetScape.game.logics.interactions;
using JetScape.game.utility;

namespace JetScape.Collisions
{
    [TestFixture]
    public class TestCollisions
    {
        private CollisionsChecker _collisionC;
        private Logics _lh;
        private IDictionary<EntityType, ISet<IEntity>> _entities;
        private Player _player;
        private SpeedHandler _speedHandler;

        [SetUp]
        public void Setup()
        {
            _lh = new Logics();
            _player = new Player(_lh);
            _entities = new Dictionary<EntityType, ISet<IEntity>>();
            _collisionC = new CollisionsChecker(_entities, _player);
            _speedHandler = new SpeedHandler(250.0, 15.0, 0);
        }

        [Test]
        public void Test()
        {
            var missile = new Missile(_lh, _player.Position, _player, _speedHandler);
            _lh.Entities[EntityType.MISSILE].Add(missile);
            _collisionC.updateCollisions();
            Assert.That(_collisionC.NextToHandle(), Is.Not.EqualTo(null));
        }
    }
}