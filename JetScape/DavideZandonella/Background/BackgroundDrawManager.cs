//Not implemented
//using Graphics2D;

using System.Drawing.Color;
using System.Collection.Generic;

using JetScape.Sprites.Sprite;

namespace JetScape.Background
{
    public class BackgroundDrawManager : BackgroundDrawer
    {

        private readonly Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();

        private readonly Color _defaultColor;

        public void SetPlaceH(Color placeHolder)
        {
            _defaultColor = placeHolder;
            _sprites.Add(PlaceholderKey, new BackgroundSprite(PlaceholderKey, defaultColor));
        }

        public void AddSprite(String name, String path)
        {
            _sprites.Add(name, new BackgroundSprite(name, defaultColor, path));
        }


        public void AddLoadedSprite(Sprite t)
        {
            _sprites.Add(t.GetName(), t);
        }

        private void Draw(Graphics2D g, string sprite, (double X, double Y) pos, int height, int width)
        {
			Sprite valueSprite;

            if (_sprites.ContainsKey(sprite))
            {
                _sprites.TryGetValue(sprite, valueSprite).Draw(g, pos, height, width);
            }
            else if (_sprites.ContainsKey(PlaceholderKey))
            {
                _sprites.TryGetValue(PlaceholderKey, valueSprite).Draw(g, pos, height, width);
            }
        }

        public void drawSprite(Graphics2D g, string sprite, (double X, double Y) pos, int height, int width)
        {
            Draw(g, sprite, pos, height, width);
        }
    }
}
