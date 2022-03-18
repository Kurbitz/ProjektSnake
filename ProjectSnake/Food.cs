using System.Drawing;

namespace ProjectSnake
{
    public abstract class Food : ICollidable, IDrawable
    {
        public Point position
        {
            get;
            private set;
        }

        public Color Color { get; }

        public Food(Point pos, Color color)
        {
            position = pos;
            Color = color;
        }

        public abstract void Draw(IRenderer graphic);
        public abstract void OnCollision(Player player);

        // Returns true if the snake's head is in the same position as the food
        public bool CheckCollision(Snake snake)
        {
            return snake.CheckCollision(position);
        }
    }
}
