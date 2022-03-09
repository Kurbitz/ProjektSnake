using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ProjectSnake
{
    public class Snake : IDrawable, ICollidable
    {
        float Speed = 1.0f;
        public List<Point> segments = new List<Point>(1);
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

        // Returns true if a snake's head collides with another snake segment
        public bool CheckCollision(Snake snake)
        {
            // Special case when checking collisions between a snake and itself to avoid colliding with it's own head
            if (ReferenceEquals(this, snake))
            {
                return snake.segments.Skip(1).Any(segment => (segments[0].X, segments[0].Y) == (segment.X, segment.Y));
            }
            // If colliding with another snake
            return snake.segments.Any(segment => (segments[0].X, segments[0].Y) == (segment.X, segment.Y));
        }
    }
}
