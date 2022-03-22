using System;
using System.Drawing;

namespace ProjectSnake
{
    internal class ValuableFood : Food
    {
        public ValuableFood(Point pos) : base(pos, Gruvbox.Yellow, 5, 2)
        {
        }

        public override void Draw(IRenderer renderer)
        {
            renderer.Draw(this);
        }
    }
}
