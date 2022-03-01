using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSnake
{
    class DietFood : Food
    {
        public DietFood(Point pos, Color color) : base(pos, color)
        {
        }

        public override void Draw(Graphics graphic)
        {
            throw new NotImplementedException();
        }

        public override void OnCollision(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
