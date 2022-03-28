using System;
using System.Collections.Generic;
using System.Linq;
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

        // EXTENDED
        private static readonly Dictionary<Keys, Direction> Player3 = new Dictionary<Keys, Direction>()
        {
            {Keys.J, Direction.Left}, {Keys.L, Direction.Right}, {Keys.I, Direction.Up}, {Keys.K, Direction.Down}
        };

        // EXTENDED
        public static readonly Controls[] ControlsBlueprints =
        {
            new Controls(Player1), new Controls(Player2), new Controls(Player3)
        };

        private readonly Dictionary<Keys, Direction> _dictionary;

        // EXTENDED
        private Random _random = new Random();

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

        // EXTENDED
        public Controls RandomControls()
        {
            // var randomControls = _dictionary.OrderBy(r => _random.Next())
            //     .ToDictionary(item => item.Key, item => item.Value);
            var keys = _dictionary.Keys.OrderBy(x => _random.Next()).ToList();
            var directions = _dictionary.Values.OrderBy(x => _random.Next()).ToList();

            var randomControls = new Dictionary<Keys, Direction>() { };
            for (int i = 0; i < 4; i++)
            {
                randomControls.Add(keys[i], directions[i]);
            }

            return new Controls(randomControls);
        }
    }
}
