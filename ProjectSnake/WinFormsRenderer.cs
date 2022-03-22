using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ProjectSnake
{
    public class WinFormsRenderer : IRenderer
    {
        private List<Food> _food = new List<Food>();
        private List<Player> _players = new List<Player>();
        private List<ScoreLabel> _scoreLabels = new List<ScoreLabel>();

        private Board _board;

        // Renderern måste veta hur stort brädet är för att kunna skala saker rätt.
        // NOTE(Johan): Detta betyder också att om brädets storlek i framtiden ändras
        // av någon anledning behöver renderern kunna få reda på det på något sätt.
        public WinFormsRenderer(Board board)
        {
            _board = board;
        }

        public void Clear()
        {
            _food.Clear();
            _players.Clear();
            _scoreLabels.Clear();
        }

        public void Draw(Food food)
        {
            _food.Add(food);
        }

        public void Draw(Player player)
        {
            _players.Add(player);
        }

        public void Draw(ScoreLabel scoreLabel)
        {
            _scoreLabels.Add(scoreLabel);
        }

        // Ritar ut all mat och alla ormar till fönstret som representeras av control och graphics.
        public void Display(Control control, Graphics graphics)
        {
            var tileWidth = control.ClientSize.Width / _board.Width;
            var tileHeight = control.ClientSize.Height / _board.Height;
            var tileSize = new Size(tileWidth, tileHeight);

            foreach (var food in _food)
            {
                var foodPixelPosition = new Point(food.position.X * tileSize.Width, food.position.Y * tileSize.Height);
                var drawingArea = new Rectangle(foodPixelPosition, tileSize);
                var brush = new SolidBrush(food.Color);

                graphics.FillRectangle(brush, drawingArea);
            }

            foreach (var player in _players)
            {
                var bodyBrush = new SolidBrush(player.Snake.ColorScheme.Body);
                foreach (var segment in player.Snake)
                {
                    var segmentPixelPosition = new Point(segment.X * tileSize.Width, segment.Y * tileSize.Height);
                    var drawingArea = new Rectangle(segmentPixelPosition, tileSize);
                    graphics.FillRectangle(bodyBrush, drawingArea);
                }
            }

            foreach (var scoreLabel in _scoreLabels)
            {
                scoreLabel.UpdateText();
            }
        }
    }
}
