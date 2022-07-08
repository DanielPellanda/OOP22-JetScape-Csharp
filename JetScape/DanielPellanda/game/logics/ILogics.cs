using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JetScape.game.utility;
using JetScape.game.logics.entities;

namespace JetScape.game.logics
{
    public interface ILogics
    {
        GameState GameState  { get; set; }

        IDictionary<EntityType, ISet<IEntity>> Entities { get; }

        Cleaner GetEntitiesCleaner();

        void UpdateAll();
    }
}
