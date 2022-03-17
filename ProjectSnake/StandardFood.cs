using System;
using System.Drawing;

namespace ProjectSnake
{
    internal class StandardFood : Food
    {
        public StandardFood(Point pos) : base(pos, Color.Red)
        {
        }

        public override void Draw(IRenderer renderer)
        {
            renderer.Draw(this);
        }

        public override void OnCollision(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
