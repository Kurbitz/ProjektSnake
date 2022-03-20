using System.Drawing;

namespace ProjectSnake
{
    public class Player : ICollidable
    {
        public Snake Snake { get; }
        private int _score;

        public static (PointF Position, Color Color)[] SnakeBlueprints { get; } =
        {
            (new PointF(1F / 3F, 1F / 3F), Color.Green), (new PointF(2F / 3F, 1F / 3F), Color.Black),
            (new PointF(1F / 3F, 2F / 3F), Color.Brown)
        };

        public Player(Point snakeStartingPos, Color snakeColor)
        {
            Snake = new Snake(snakeStartingPos, snakeColor);
        }

        public void OnCollision(Food food)
        {
            _score += food.Points;
            Snake.Grow(food.LengthFactor);
        }

        public void OnCollision(Player player)
        {
            Snake.Die();
            player._score += 5;
        }

        public bool CheckCollision(Snake snake)
        {
            return Snake.CheckCollision(snake);
        }

        public bool CheckCollision(Board board)
        {
            return Snake.CheckCollision(board);
        }
    }
}
