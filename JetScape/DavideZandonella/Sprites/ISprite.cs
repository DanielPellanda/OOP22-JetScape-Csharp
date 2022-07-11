// Graphics2D not implemented
//using Graphics2D;
using System.ValueTuple;

namespace JetScape.Sprites
{
    public interface ISprite
    {
        string GetName();

        void Draw(Graphics2D g, (double, double) pos, params int[] sizes);
    }
}
