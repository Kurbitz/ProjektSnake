using System.Collections.Generic;
using System.Drawing;

namespace ProjectSnake
{
    internal class Snake : IDrawable, ICollidable
    {
        float Speed = 1.0f;
        List<Point> segments = new List<Point>(1);
        Color _color;

        public Snake(Point startingPosition, Color color)
        {
            segments[0] = startingPosition;
            _color = color;
        }

        void GrowHead(int sizeChange)
        {
            throw new System.NotImplementedException();
        }

        void GrowTail(int sizeChange)
        {
            throw new System.NotImplementedException();
        }

        public void Draw(Graphics graphic)
        {
            throw new System.NotImplementedException();
        }

        public void OnCollision(Player player)
        {
            throw new System.NotImplementedException();
        }
    }
}