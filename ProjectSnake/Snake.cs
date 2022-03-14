using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ProjectSnake
{
    public class Snake : IDrawable, ICollidable
    {
        private float Speed = 1.0f;
        private List<Point> _segments = new List<Point>(1);
        private Color _color;

        public Snake(Point startingPosition, Color color)
        {
            _segments[0] = startingPosition;
            _color = color;
        }

        private void GrowHead(int sizeChange)
        {
            throw new System.NotImplementedException();
        }

        private void GrowTail(int sizeChange)
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
                return snake._segments.Skip(1).Any(segment => CheckCollision(segment));
            }

            // If colliding with another snake
            return snake._segments.Any(segment => CheckCollision(segment));
        }

        // Returns true if a snake's head occupies a position
        public bool CheckCollision(Point position) => _segments[0] == position;
    }
}
