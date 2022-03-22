using System.Drawing;

namespace ProjectSnake
{
    public struct SnakeColorScheme
    {
        public Color Head;
        public Color Body;

        public SnakeColorScheme(Color head, Color body)
        {
            Head = head;
            Body = body;
        }
    }
}
