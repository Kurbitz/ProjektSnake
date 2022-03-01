using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSnake
{
    interface ICollidable
    {
        void OnCollision(Player player);
    }
}
