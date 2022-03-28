using System;
using System.Drawing;

namespace ProjectSnake
{
    internal class DietFood : Food
    {
        public DietFood(Point pos) : base(pos, Gruvbox.Gray, 1, -1)
        {
        }
    }
}
