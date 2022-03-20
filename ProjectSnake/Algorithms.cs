using System.Collections.Generic;
using System.Linq;

namespace ProjectSnake
{
    public static class Algorithms
    {
        // Roterar alla element i en lista ett steg åt vänster.
        // T.ex. [1, 2, 3, 4, 5] -> [2, 3, 4, 5, 1]
        public static void RotateOneStepLeft<T>(this IList<T> list)
        {
            var tmp = list.Last();
            for (var i = 1; i < list.Count; ++i)
            {
                list[i - 1] = list[i];
            }

            list[list.Count - 1] = tmp;
        }
    }
}
