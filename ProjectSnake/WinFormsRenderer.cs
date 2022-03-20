using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ProjectSnake
{
    public class WinFormsRenderer : IRenderer
    {
        private List<Food> _food = new List<Food>();
        private List<Snake> _snakes = new List<Snake>();
        private List<Player> _players = new List<Player>();

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
            _snakes.Clear();
            _players.Clear();
        }

        public void Draw(Food food)
        {
            _food.Add(food);
        }

        public void Draw(Snake snake)
        {
            _snakes.Add(snake);
        }

        public void Draw(Player player)
         {
            _players.Add(player);
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

            foreach (var snake in _snakes)
            {
                var brush = new SolidBrush(snake.Color);
                foreach (var segment in snake)
                {
                    var segmentPixelPosition = new Point(segment.X * tileSize.Width, segment.Y * tileSize.Height);
                    var drawingArea = new Rectangle(segmentPixelPosition, tileSize);
                    graphics.FillRectangle(brush, drawingArea);
                }
            }
        }
    }
}
