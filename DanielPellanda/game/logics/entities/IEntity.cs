using DanielPellanda.game.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanielPellanda.game.logics.entities
{
    public interface IEntity
    {
        //double Y_TOP_LIMIT = 0;
        //double Y_LOW_LIMIT = GameWindow.GAME_SCREEN.getHeight() - (GameWindow.GAME_SCREEN.getTileSize() * 2);

        bool IsVisible();
        bool IsOnScreenBounds();
        bool IsOnClearArea();
        bool IsOnSpawnArea();
        Tuple<double, double> GetPosition();

        //Hitbox getHitbox();

        EntityType EntityType();
        void Reset();
        void Clean();
        void Update();
    }
}
