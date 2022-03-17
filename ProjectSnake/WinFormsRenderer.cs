using System.Collections.Generic;

namespace ProjectSnake
{
    public class WinFormsRenderer : IRenderer
    {
        private List<Food> _food = new List<Food>();
        private List<Snake> _snakes = new List<Snake>();

        public void Draw(Food food)
        {
            _food.Add(food);
        }

        public void Draw(Snake snake)
        {
            _snakes.Add(snake);
        }
    }
}
