using System.Drawing;

namespace ProjectSnake
{
    public class Player
    {
        public Snake Snake { get; }
        private int score;

        public static (PointF Position, Color Color)[] SnakeBlueprints { get; } =
        {
            (new PointF(1F / 3F, 1F / 3F), Color.Green), (new PointF(2F / 3F, 1F / 3F), Color.Black),
            (new PointF(1F / 3F, 2F / 3F), Color.Brown)
        };

        public Player(Point snakeStartingPos, Color snakeColor)
        {
            Snake = new Snake(snakeStartingPos, snakeColor);
        }
    }
}
