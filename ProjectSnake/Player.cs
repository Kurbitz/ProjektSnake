using System.Drawing;

namespace ProjectSnake
{
    public class Player
    {
        public Snake Snake { get; }
        private int score;

        public Player(Point snakeStartingPos, Color snakeColor)
        {
            Snake = new Snake(snakeStartingPos, snakeColor);
        }
    }
}
