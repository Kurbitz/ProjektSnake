using System.Collections;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ProjectSnake
{
    public enum Direction { Up, Down, Right, Left }

    public class Snake : IEnumerable<Point>
    {
        // _speed räknas i distance per step. Varje step ökar ormens distance baserat på _speed.
        // När ormen har rört sig en längre distance än DistancePerMove så flyttar den på sig ett steg.
        private const float InitialSpeed = 0.2f;
        private const float DistancePerMove = 1.0f;
        private float _speed = InitialSpeed;
        private float _distanceTraveledSinceLastMove = 0.0f;

        private readonly List<Point> _segments = new List<Point>(1);
        public SnakeColorScheme ColorScheme { get; }

        private Direction _facingDirection;
        private Direction _lastMoveDirection;
        private int _amountToGrow;
        private bool _canGrow;

        // Assumes the Head is the last segment.
        private Point Head => _segments.Last();

        public bool IsAlive { get; set; } = true;

        public Snake(Point startingPosition, SnakeColorScheme colorScheme)
        {
            _segments.Add(startingPosition);
            _amountToGrow = 4;
            ColorScheme = colorScheme;
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

        // Flyttar ormen om den har färdats tillräckligt långt.
        public void Step()
        {
            if (!IsAlive)
            {
                return;
            }

            _distanceTraveledSinceLastMove += _speed;
            if (_distanceTraveledSinceLastMove > DistancePerMove)
            {
                _distanceTraveledSinceLastMove -= DistancePerMove;
                MoveInFacingDirection();
            }
        }

        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Down:
                case Direction.Up:
                    if (_lastMoveDirection == Direction.Left || _lastMoveDirection == Direction.Right)
                    {
                        _facingDirection = direction;
                    }

                    break;
                case Direction.Left:
                case Direction.Right:
                    if (_lastMoveDirection == Direction.Up || _lastMoveDirection == Direction.Down)
                    {
                        _facingDirection = direction;
                    }

                    break;
            }
        }

        private void MoveInFacingDirection()
        {
            var stepForward = ToUnitStepSize(_facingDirection);
            var newHead = Head + stepForward;
            if (_amountToGrow > 0)
            {
                _segments.Add(newHead);
                --_amountToGrow;
            }
            else
            {
                _segments.RotateOneStepLeft();
                _segments[_segments.Count - 1] = newHead;
            }

            _canGrow = true;
            _lastMoveDirection = _facingDirection;
        }

        public void Grow(int sizeChange)
        {
            if (!IsAlive)
            {
                return;
            }

            if (sizeChange < 0)
            {
                Shrink(-sizeChange);
            }
            else if (_canGrow)
            {
                _amountToGrow += sizeChange;
                _canGrow = false;
            }
        }

        private void Shrink(int amount)
        {
            if (amount >= _segments.Count)
            {
                IsAlive = false;
                _segments.Clear();
            }
            else
            {
                // Assumes the head is at the last index.
                _segments.RemoveRange(0, amount);
            }
        }

        public void Clear()
        {
            _segments.Clear();
        }

        // Kollar om ormen krockar med en annan orm eller sig själv
        public bool CheckCollision(Snake snake)
        {
            // Om ormen är död så ska den inte kunna krocka
            if (!IsAlive)
            {
                return false;
            }

            // Om ormen kollar krock med sig själv, kollar om det finns fler än ett segment på samma position
            if (ReferenceEquals(this, snake))
            {
                return _segments.Count != _segments.Distinct().Count();
            }

            // Kolla om den krockar med den andra ormen
            return snake._segments.Any(segment => CheckCollision(segment));
        }

        // Returns true if a snake's head occupies a position
        public bool CheckCollision(Point position)
        {
            if (!IsAlive)
            {
                return false;
            }

            return Head == position;
        }

        public bool CheckCollision(Board board)
        {
            if (!IsAlive)
            {
                return false;
            }

            return Head.X < 0 || Head.Y < 0 || Head.X >= board.Width || Head.Y >= board.Height;
        }

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
