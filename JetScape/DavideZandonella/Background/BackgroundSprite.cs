// Not implemented
/*
using Graphics2D;
using BufferedImage;
using ImageIO;
*/
using System;
using System.Drawing.Color;
using System.File;
using System.IO;
using System.ValueTuple;

using JetScape.Sprites.AbstractSprite;
using JetScape.Sprites.Sprite;

namespace JetScape.Background
{
    public class BackgroundSprite : AbstractSprite, Sprite
    {
        private readonly BufferedImage? _image;

        private readonly Color _placeHolder;

        private readonly string _name = "unknown";

        public BackgroundSprite(string name, Color placeHolder)
	    {
            _name = name;
            _placeHolder = placeHolder;
        }

        public BackgroundSprite(string name, Color placeHolder, string path) : this(name, placeHolder)
	    {
            _image = BackgroundSprite.Load(
                    AbstractSprite.GetDefaultSpriteDirectory() + path);
        }

        public static BufferedImage Load(string path)
	    {
            BufferedImage? loaded = null;
            try
            {
                loaded = ImageIO.Read(new File(path));
            }
            catch (IOException e) {
                Console.WriteLine(e.Message);
            }
            return loaded;
        }

        public string GetName() => _name;

        public void Draw(Graphics2D g, (double X, double Y) pos, params int[] sizes)
	    {
            int height = sizes[0];
            int width;

            if (sizes.Length == 2)
                width = sizes[1];
            else
                width = height;

            if (_image.HasValue)
            {
                //Not implemented: left as Java version
                g.DrawImage(_image, (int) Math.Round(pos.X),
                        (int) Math.Round(pos.Y), width, height, null);
            }
            else
            {
                //Not implemented: left as Java version
                g.SetColor(_placeHolder);
                g.FillRect((int) Math.Round(pos.X),
                        (int) Math.Round(pos.Y), width, height);
            }
        }
    }
}
