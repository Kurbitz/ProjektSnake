namespace ProjectSnake
{
    public interface IRenderer
    {
        void Draw(Food food);
        void Draw(Snake snake);
        void Draw(Player player);
    }
}
