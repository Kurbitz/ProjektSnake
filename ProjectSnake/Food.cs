using System.Drawing;

namespace ProjectSnake
{
    internal abstract class Food : ICollidable, IDrawable
    {
        public Point position
        {
            get;
            private set;
        }

        protected Color color;

        public Food(Point pos, Color color)
        {
            position = pos;
            this.color = color;
        }

        public abstract void Draw(Graphics graphic);
        public abstract void OnCollision(Player player);

        // Returns true if the snake's head is in the same position as the food
        public bool CheckCollision(Snake snake)
        {
            return snake.CheckCollision(position);
        }
    }
}
