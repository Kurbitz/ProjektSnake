using System;
using System.Drawing;

namespace ProjectSnake
{
    internal class DietFood : Food
    {
        public DietFood(Point pos) : base(pos, Color.Aqua, 1, -1)
        {
        }

        public override void Draw(IRenderer renderer)
        {
            renderer.Draw(this);
        }
    }
}
