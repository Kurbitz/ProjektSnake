using System;
using System.Drawing;

namespace ProjectSnake
{
    internal class DietFood : Food
    {
        public DietFood(Point pos) : base(pos, Color.Aqua)
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
