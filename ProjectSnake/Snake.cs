using System.Collections;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ProjectSnake
{
    public class Snake : IDrawable, ICollidable, IEnumerable<Point>
    {
        public enum Direction
        {
            Up, Left, Down, Right
        }

        private float Speed = 1.0f;
        private List<Point> _segments = new List<Point>(1);
        public Color Color { get; }

        private Direction _facingDirection;
        private Direction _lastMoveDirection;

        public Snake(Point startingPosition, Color color)
        {
            _segments.Add(startingPosition);
            Color = color;
        }

        // Ger tillbaka en storlek som pekar ett steg i directions riktning.
        private Size ToUnitStepSize(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Size(0, -1);
                case Direction.Left:
                    return new Size(-1, 0);
                case Direction.Down:
                    return new Size(0, 1);
                case Direction.Right:
                    return new Size(1, 0);
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction,
                        "Direction has a value that's not handled by the switch case.");
            }
        }

        private void GrowHead(int sizeChange)
        {
            throw new System.NotImplementedException();
        }

        private void GrowTail(int sizeChange)
        {
            throw new System.NotImplementedException();
        }

        public void Draw(IRenderer renderer)
        {
            renderer.Draw(this);
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

        public IEnumerator<Point> GetEnumerator()
        {
            return _segments.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
