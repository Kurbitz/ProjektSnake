using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSnake
{
    class StandardFood : Food
    {
        public StandardFood(Point pos) : base(pos, Color.Red)
        {

        }

        public override void Draw(Graphics graphic)
        {
            graphic.Clear(color);
        }

        public override void OnCollision(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
