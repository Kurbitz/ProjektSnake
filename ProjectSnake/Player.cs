using System.Drawing;

namespace ProjectSnake
{
    public class Player : ICollidable, IDrawable
    {
        public Snake Snake { get; }
        public int Score;

        public static (PointF Position, SnakeColorScheme ColorScheme)[] SnakeBlueprints { get; } =
        {
            (new PointF(1F / 3F, 1F / 3F), new SnakeColorScheme(Gruvbox.Red, Gruvbox.DarkRed)),
            (new PointF(2F / 3F, 1F / 3F), new SnakeColorScheme(Gruvbox.Green, Gruvbox.DarkGreen)),
            (new PointF(1F / 3F, 2F / 3F), new SnakeColorScheme(Gruvbox.Blue, Gruvbox.DarkBlue))
        };

        public Controls Controls;

        public Player(Point snakeStartingPos, SnakeColorScheme snakeColorScheme)
        {
            Snake = new Snake(snakeStartingPos, snakeColorScheme);
        }

        public void OnCollision(Food food)
        {
            Score += food.Points;
            Snake.Grow(food.LengthFactor);
        }

        public void OnCollision(Player player)
        {
            Snake.IsAlive = false;
            player.Score += 5;
        }

        public bool CheckCollision(Snake snake)
        {
            return Snake.CheckCollision(snake);
        }

        public bool CheckCollision(Board board)
        {
            return Snake.CheckCollision(board);
        }

        public void Draw(IRenderer renderer)
        {
            renderer.Draw(this);
        }
    }
}
