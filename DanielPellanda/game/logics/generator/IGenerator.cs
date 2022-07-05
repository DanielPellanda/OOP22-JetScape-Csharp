using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DanielPellanda.game.logics.generator
{
    public interface IGenerator
    {
        /*
        void setZapperRayCreator(BiFunction<Pair<ZapperBase, ZapperBase>, Pair<Double, Double>, ZapperRay> zapperr);
        void setZapperBaseCreator(Function<Pair<Double, Double>, ZapperBase> zapperb);
        void setMissileCreator(Function<Pair<Double, Double>, Missile> missile);
        void setShieldCreator(Function<Pair<Double, Double>, Shield> shield);
        void setTeleportCreator(Function<Pair<Double, Double>, Teleport> teleport);
        void setCoinCreator(Function<Pair<Double, Double>, Coin> coin);
        */

        bool IsRunning();
        bool IsWaiting();

        //void initialize() throws FileNotFoundException, JsonException, FormatException;

        Mutex Mutex { get; }
        void Start();
        void Terminate();
        void Stop();
        void Pause();
        void Resume();
    }
}
