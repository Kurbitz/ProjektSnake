using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Windows.Forms;

namespace ProjectSnake
{
    public class Controls
    {
        static Dictionary<Keys, Direction> _player1 = new Dictionary<Keys, Direction>()
        {
            {Keys.Left, Direction.Left},
            {Keys.Right, Direction.Right},
            {Keys.Up, Direction.Up},
            {Keys.Down, Direction.Down}
        };

        static Dictionary<Keys, Direction> _player2 = new Dictionary<Keys, Direction>()
        {
            {Keys.A, Direction.Left}, {Keys.D, Direction.Right}, {Keys.W, Direction.Up}, {Keys.S, Direction.Down}
        };

        public static Controls[] ControlsBlueprints = {new Controls(_player1), new Controls(_player2)};

        Dictionary<Keys, Direction> _dictionary;

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
