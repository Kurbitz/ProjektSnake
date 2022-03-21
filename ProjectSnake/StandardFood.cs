using System;
using System.Drawing;

namespace ProjectSnake
{
    internal class StandardFood : Food
    {
        public StandardFood(Point pos) : base(pos, Color.FromArgb(248, 243, 214), 1, 1)
        {
        }

        public override void Draw(IRenderer renderer)
        {
            renderer.Draw(this);
        }
    }
}
