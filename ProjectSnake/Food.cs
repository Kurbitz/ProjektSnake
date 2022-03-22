using System.Drawing;

namespace ProjectSnake
{
    public abstract class Food : ICollidable, IDrawable
    {
        public int Points { get; }

        public int LengthFactor { get; }

        public bool IsActive { get; private set; } = true;

        public Point Position
        {
            get;
            private set;
        }

        public Color Color { get; }

        protected Food(Point pos, Color color, int points, int lengthFactor)
        {
            Position = pos;
            Color = color;
            Points = points;
            LengthFactor = lengthFactor;
        }

        public abstract void Draw(IRenderer graphic);

        public void OnCollision(Player player)
        {
            player.OnCollision(this);
            IsActive = false;
        }

        // Returns true if the snake's head is in the same position as the food
        public bool CheckCollision(Snake snake)
        {
            return snake.CheckCollision(Position);
        }
    }
}
