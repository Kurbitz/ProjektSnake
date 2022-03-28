using System.Drawing;
using System.Windows.Forms;

namespace ProjectSnake
{
    public class Player : ICollidable, IDrawable
    {
        public Snake Snake { get; }
        public int Score;
        private bool _randomControls;
        private Timer _resetRandomControlsTimer = new Timer();

        public static (PointF Position, Direction initialDirection, SnakeColorScheme ColorScheme)[] SnakeBlueprints { get; } =
        {
            (new PointF(1F / 3F, 1F / 3F), Direction.Right ,new SnakeColorScheme(Gruvbox.Red, Gruvbox.DarkRed)),
            (new PointF(2F / 3F, 1F / 3F), Direction.Down ,new SnakeColorScheme(Gruvbox.Green, Gruvbox.DarkGreen)),
            (new PointF(1F / 3F, 2F / 3F), Direction.Left ,new SnakeColorScheme(Gruvbox.Blue, Gruvbox.DarkBlue))
        };

        public Controls StandardControls { get; }
        
        public Controls CurrentControls { get; private set; }

        public bool RandomControls
        {
            get => _randomControls;
            private set
            {
                if (value)
                {
                    CurrentControls = StandardControls.RandomControls();
                    _resetRandomControlsTimer.Start();
                }
                else
                {
                    CurrentControls = StandardControls;
                    _resetRandomControlsTimer.Stop();
                }

                _randomControls = value;
            }
        }

        public Player(Snake snake, Controls controls)
        {
            Snake = snake;
            StandardControls = controls;
            CurrentControls = controls;
            _resetRandomControlsTimer.Tick += (obj, eventArgs ) => RandomControls = false;
            _resetRandomControlsTimer.Interval = 3000;
        }

        public void OnCollision(Food food)
        {
            Score += food.Points;
            Snake.Grow(food.LengthFactor);
        }

        public void OnCollision(RandomizeControlsFood food)
        {
            Score += food.Points;
            Snake.Grow(food.LengthFactor);
            RandomControls = true;
        }

        public void OnCollision(Player player)
        {
            Snake.IsAlive = false;
            if(!ReferenceEquals(this, player))
            {
                player.Score += 5;
            }
        }

        public bool CheckCollision(Snake snake)
        {
            return Snake.CheckCollision(snake);
        }

        public bool CheckCollision(Board board)
        {
            return Snake.CheckCollision(board);
        }

        public void Draw(IRenderer renderer)
        {
            renderer.Draw(this);
        }
    }
}
