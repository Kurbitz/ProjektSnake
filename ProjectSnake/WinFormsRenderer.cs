using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ProjectSnake
{
    public class WinFormsRenderer : IRenderer
    {
        private List<Food> _food = new List<Food>();
        private List<Snake> _snakes = new List<Snake>();

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
        }

        public void Draw(Food food)
        {
            _food.Add(food);
        }

        public void Draw(Snake snake)
        {
            _snakes.Add(snake);
        }

        // Ritar ut all mat och alla ormar till fönstret som representeras av control och graphics.
        public void Display(Control control, Graphics graphics)
        {
            throw new NotImplementedException();
        }
    }
}
