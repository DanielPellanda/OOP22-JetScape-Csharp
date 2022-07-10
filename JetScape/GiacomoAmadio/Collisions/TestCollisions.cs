using NUnit.Framework;
using JetScape.game.logics;
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
        private Player _player;
        private SpeedHandler _speedHandler;

        [SetUp]
        public void Setup()
        {
            _lh = new Logics();
            _player = new Player(_lh);
            _collisionC = new CollisionsChecker(_lh.Entities, _player);
            _speedHandler = new SpeedHandler(250.0, 15.0, 0);
        }

        [Test]
        public void Test()
        {
            var missile = new Missile(_lh, _player.Position, _player, _speedHandler);
            _lh.Entities[EntityType.MISSILE].Add(missile);
            _collisionC.UpdateCollisions();
            Assert.That(_collisionC.NextToHandle(), Is.Not.EqualTo(null));
        }
    }
}