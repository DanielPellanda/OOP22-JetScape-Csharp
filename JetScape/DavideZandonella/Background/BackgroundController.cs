// Not implemented
using Graphics2D;

using System.Drawing;
using System.Collections.Generic;

// Not implemented here
/*
using game.frame.GameWindow;
using game.logics.interactions.SpeedHandler;
using game.utility.debug.Debugger;
*/

namespace JetScape.Background
{
    public class BackgroundController : Background
    {

        private static readonly string SpritePath = "background" + (string) Path.DirectorySeparatorChar;

		// The Java version has a grey / dark blue handmade color difficult to reproduce
        private static readonly Color PlaceHolder = Color.Grey;

        private const readonly string KeySprite1 = "background1";

        private const readonly string KeySprite2 = "background2";

        private static readonly double LadderGeneration = 0.15;

        private static readonly int ScreenWidth = GameWindow.GameScreen.GetWidth();

        private static readonly (double X, double Y) StartPosition = (0.0, 0.0);

        private readonly (double X, double Y) position = StartPosition.Copy();

        private readonly IBackgroundDrawer _drawMgr = new BackgroundDrawManager();
        private readonly var _rand = new Random();
        private readonly SpeedHandler _movement;

        /// FLAGS ///
        private bool _visible;
        private bool _toBeGenerated;
        private bool _toBeShifted;
        private readonly IDictionary<BoxPos, bool> _boxVisible;
        private readonly IDictionary<BoxPos, string?> _boxSprite;


        public BackgroundController(SpeedHandler speed) {

            _movement = speed;
            _drawMgr.SetPlaceH(PlaceHolder);
            _drawMgr.AddSprite(KeySprite1, SpritePath + "background_1.png");
            _drawMgr.AddSprite(KeySprite2, SpritePath + "background_2.png");

            _boxVisible = new Dictionary<BoxPos, bool>() {
                {BoxPos.Left, false},
                {BoxPos.Central, true},
                {BoxPos.Right, false}
            };
            _boxSprite = new Dictionary<BoxPos, string?>(3);

		    foreach (BoxPos pos in BoxPos.Values())
            {
                _boxSprite.Add(pos, null));
            }

            _boxSprite.Add(BoxPos.Central, KeySprite1?);
            _boxSprite.Add(BoxPos.Right, KeySprite1?);

            SetVisibility(true);
        }


        public sealed void SetVisibility(bool v)
        {
            _visible = v;
        }

        public sealed bool IsVisible()
        {
            return _visible;
        }

        public void Reset()
        {
            _position.Set(StartPosition.GetX(), StartPosition.GetY());
            _movement.ResetSpeed();
        }

        public void Update()
        {
            UpdateFlags();
            if (IsVisible())
            {
                if (_toBeGenerated)
                {
                    _boxSprite.Add(BoxPos.Right,
                            rand.NextDouble() > LadderGeneration ? KeySprite1 : KeySprite2);
                }
                if (_toBeShifted)
                {
                    ShiftBox();
                    _toBeShifted = false;
                }
                if (_position.GetX() > -ScreenWidth * 2)
                {
                    _position.SetX(_position.GetX() - _movement.GetXSpeed() / GameWindow.FpsLimit);
                }
            }
        }

        public void Draw(Graphics2D g)
        {
            bool defaultValue;

            if (IsVisible())
            {
                _boxSprite.EntrySet()
                        .Where(box => _boxVisible.TryGetValue(box.GetKey(), out defaultValue))
                        .ForEach(box => _drawMgr.DrawSprite(g,
                                box.GetValue() ?? BackgroundDrawer.PlaceholderKey,
                                Calculate(box.GetKey()),
                                GameWindow.GameScreen.GetHeight(),
                                GameWindow.GameScreen.GetWidth()));
            }
        }

        private void ShiftBox() {

			string? defaultValue;

            _boxVisible.Clear();

            _boxVisible.Add(BoxPos.Left, true);
            _boxVisible.Add(BoxPos.Central, true);
            _boxVisible.Add(BoxPos.Right, false);

            _boxSprite.Add(BoxPos.Left, _boxSprite.TryGetValue(BoxPos.Central, out defaultValue));
            _boxSprite.Add(BoxPos.Central, _boxSprite.TryGetValue(BoxPos.Right, out defaultValue));

            var temp = Calculate(BoxPos.Right);
            _position.Set(temp);
        }

        private (double, double) Calculate(BoxPos box)
        {
            (double, double) newPos;
            switch (box)
            {
                case Left:
                    newPos = (_position.GetX() - ScreenWidth, _position.GetY());
                    break;
                case Right:
                    newPos = (_position.GetX() + ScreenWidth, _position.GetY());
                    break;
                default:
                    newPos = _position.Copy();
                    break;
            }
            return newPos;
        }

        public void DrawCoordinates(Graphics2D g)
        {
            int xShift = (int) Math.Round(_position.GetX())
                    + (int) Math.Round(GameWindow.GameScreen.GetTileSize() * 0.88);
            int yShiftDrawnX = (int) Math.Round(_position.GetY())
                    + GameWindow.GameScreen.GetTileSize();
            int yShiftDrawnY = yShiftDrawnX + 10;

            if (GameWindow.GameDebugger.IsFeatureEnabled(Debugger.Option.BackgroundCoordinates) && IsVisible())
            {
                g.SetColor(Debugger.DebugColor);
                g.SetFont(Debugger.DebugFont);

                g.DrawString("X:" + Math.Round(_position.GetX()), xShift, yShiftDrawnX);
                g.DrawString("Y:" + Math.Round(_position.GetY()), xShift, yShiftDrawnY);
            }
        }

        private void UpdateFlags()
        {
            if (_position.GetX() <= 0)
            {
                _toBeGenerated = true;
                _toBeShifted = true;
            }
            else
            {
                _toBeGenerated = false;
            }
        }

        public override string ToString()
        {
            return "Background"
                    + "[L: X:" + Math.Round(Calculate(BoxPos.Left).GetX())
                    +  " - Y:" + Math.Round(Calculate(BoxPos.Left).GetY()) + "]\n" + "           "
                    + "[C: X:" + Math.Round(_position.getX())
                    +  " - Y:" + Math.Round(_position.getY()) + "]\n" + "           "
                    + "[R: X:" + Math.Round(Calculate(BoxPos.Right).GetX())
                    +  " - Y:" + Math.Round(Calculate(BoxPos.Right).GetY()) + "]";
        }

        private enum BoxPos
        {
            Left, Central, Right;
        }
    }
}
