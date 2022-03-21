using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ProjectSnake
{
    public class Player : ICollidable, IDrawable
    {
        public Snake Snake { get; }
        public int Score;

        public static (PointF Position, Color Color)[] SnakeBlueprints { get; } =
        {
            (new PointF(1F / 3F, 1F / 3F), Color.FromArgb(208, 21, 26)),
            (new PointF(2F / 3F, 1F / 3F), Color.FromArgb(146, 146, 48)),
            (new PointF(1F / 3F, 2F / 3F), Color.FromArgb(61, 131, 133))
        };
        public Controls controls;
        
        public Player(Point snakeStartingPos, Color snakeColor)
        {
            Snake = new Snake(snakeStartingPos, snakeColor);
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
