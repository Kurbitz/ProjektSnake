using System.Drawing;

namespace ProjectSnake
{
    public class Player
    {
        public Snake snake;
        private int score;

        public Player(Point snakeStartingPos, Color snakeColor)
        {
            snake = new Snake(snakeStartingPos, snakeColor);
        }
    }
}
