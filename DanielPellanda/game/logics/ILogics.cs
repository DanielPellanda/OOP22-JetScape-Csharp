using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanielPellanda.game.logics
{
    public interface ILogics
    {
        Cleaner GetEntitiesCleaner();

        void UpdateAll();
    }
}
