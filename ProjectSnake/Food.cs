using System.Drawing;

namespace ProjectSnake
{
    abstract class Food : ICollidable, IDrawable
    {
        public Point position
        {
            get; private set;
        }

        protected Color color;

        public Food(Point pos, Color color)
        {
            position = pos;
            this.color = color;
        }

        public abstract void Draw(Graphics graphic);
        public abstract void OnCollision(Player player);
    }
}