﻿using System;
using System.Drawing;

namespace ProjectSnake
{
    internal class ValuableFood : Food
    {
        public ValuableFood(Point pos) : base(pos, Color.Gold)
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
