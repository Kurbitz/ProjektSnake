using System.Collections.Generic;
using System.Linq;

namespace ProjectSnake
{
    public static class Algorithms
    {
        // Roterar alla element i en lista ett steg åt höger.
        // T.ex. [1, 2, 3, 4, 5] -> [5, 1, 2, 3, 4]
        public static void RotateOneStepRight<T>(this IList<T> list)
        {
            var tmp = list.Last();
            for (var i = list.Count - 2; i >= 0; --i)
            {
                list[i + 1] = list[i];
            }

            list[0] = tmp;
        }
    }
}
