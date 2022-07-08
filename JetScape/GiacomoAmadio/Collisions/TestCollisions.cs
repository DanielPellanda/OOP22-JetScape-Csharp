using NUnit.Framework;
using System.Collections.Generic;
using JetScape.game.logics;
using JetScape.game.logics.entities;
using JetScape.game.logics.entities.player;
using JetScape.game.utility;

namespace JetScape.Collisions
{
    [TestFixture]
    public class TestCollisions
    {
        private CollisionsChecker _collisionC;
        private Logics _lh;
        private Dictionary<EntityType, ISet<Entity>> _entities;
        private Player _player;

        [SetUp]
        public void Setup()
        {
            _lh = new Logics();
            _player = new Player(_lh);
            _entities = new Dictionary<EntityType, ISet<Entity>>();
            _collisionC = new CollisionsChecker(_entities, _player);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}