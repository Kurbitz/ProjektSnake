using System.Drawing;

namespace ProjectSnake
{
    internal class StandardFood : Food
    {
        public StandardFood(Point pos) : base(pos, Gruvbox.White, 1, 1)
        {
        }
    }
}
