// Not implemented
//using Graphics2D;

using Color;
using JetScape.Sprites.Sprite;

namespace JetScape.Background {

    public interface IBackgroundDrawer {

        const string PlaceholderKey = "placeholder";

        void SetPlaceH(Color placeH);

        void AddSprite(string name, string path);

        void AddLoadedSprite(Sprite s); 

        void DrawSprite(Graphics2D g, string sprite, (double X, double Y) pos, int height, int width);
    }
}
