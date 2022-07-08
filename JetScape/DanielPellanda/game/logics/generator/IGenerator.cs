using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using JetScape.game.logics;

namespace JetScape.game.logics.generator
{
    public interface IGenerator
    {
        //ZapperRayFactory CreateZRay { set; };
        //ZapperBaseFactory CreateZBase { set; };
        MissileFactory CreateMissile { set; }
        ShieldFactory CreateShield { set; }
        //TeleportFactory CreateTeleport { set; }
        //CoinFactory CreateCoins { set; }

        bool IsRunning { get; }
        bool IsWaiting { get; }

        void Initialize();

        void Start();
        void Terminate();
        void Stop();
        void Pause();
        void Resume();
    }
}
