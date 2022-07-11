// Not implemented
/*
using Graphics2D;
using BufferedImage;
using ImageIO;
*/
using System.IO;
using System.File;
using System.IOException;
using System.ValueTuple;

namespace JetScape.Sprites
{
    public abstract class AbstractSprite : Sprite
    {
        private static const string Separator = (string) Path.DirectorySeparatorChar;

        // Not implemented: System.getProperty("user.dir")
        private static const string DefaultSpriteDirectory = System.getProperty("user.dir")
                + Separator + "res" + Separator + "game" + Separator + "sprites" + Separator;

        public static string GetSeparator() => AbstractSprite.Separator;

        public static String GetDefaultSpriteDirectory() => AbstractSprite.DefaultSpriteDirectory;

        public static BufferedImage Load(string path)
        {
            BufferedImage? loaded = null;
            try
            {
                loaded = ImageIO.Read(new File(path));
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            return loaded;
        }

        public abstract string GetName();

        public abstract void Draw(Graphics2D g, (double X, double Y) pos, params int[] sizes);
    }
}
