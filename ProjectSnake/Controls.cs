using System.Collections.Generic;
using System.Windows.Forms;

namespace ProjectSnake
{
    public class Controls
    {
        private static readonly Dictionary<Keys, Direction> Player1 = new Dictionary<Keys, Direction>()
        {
            {Keys.Left, Direction.Left},
            {Keys.Right, Direction.Right},
            {Keys.Up, Direction.Up},
            {Keys.Down, Direction.Down}
        };

        private static readonly Dictionary<Keys, Direction> Player2 = new Dictionary<Keys, Direction>()
        {
            {Keys.A, Direction.Left}, {Keys.D, Direction.Right}, {Keys.W, Direction.Up}, {Keys.S, Direction.Down}
        };

        private static readonly Dictionary<Keys, Direction> Player3 = new Dictionary<Keys, Direction>()
        {
            {Keys.J, Direction.Left}, {Keys.L, Direction.Right}, {Keys.I, Direction.Up}, {Keys.K, Direction.Down}
        };
        
        public static readonly Controls[] ControlsBlueprints = {new Controls(Player1), new Controls(Player2), new Controls(Player3)};

        private readonly Dictionary<Keys, Direction> _dictionary;

        public Controls(Dictionary<Keys, Direction> d)
        {
            _dictionary = d;
        }

        public Direction? ToDirection(Keys input)
        {
            Direction ourDirection;
            var res = _dictionary.TryGetValue(input, out ourDirection);
            if (res)
            {
                return ourDirection;
            }

            return null;
        }
    }
}
