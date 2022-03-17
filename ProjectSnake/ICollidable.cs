namespace ProjectSnake
{
    internal interface ICollidable
    {
        void OnCollision(Player player);

        bool CheckCollision(Snake snake);
    }
}
