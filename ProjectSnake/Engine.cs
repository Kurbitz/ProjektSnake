using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProjectSnake
{
    internal class Engine
    {
        private MainForm _main = new MainForm();
        private WinFormsRenderer _renderer;
        private Timer _timer = new Timer();
        private List<Food> foods = new List<Food>();
        private Player[] _players;
        private Board board;

        public void Run()
        {
            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            board.Width = 40;
            board.Height = 30;

            foods.Add(new StandardFood(new Point(board.Width / 2, board.Height / 2)));

            _renderer = new WinFormsRenderer(board);

            _players = InitializePlayers(2);

            _main.Paint += Draw;
            _timer.Tick += TimerEvent;
            _timer.Interval = 1000 / 60;
            _timer.Start();
            Application.Run(_main);
        }

        private Player[] InitializePlayers(int count)
        {
            Debug.Assert(count <= Player.SnakeBlueprints.Length);

            var players = new Player[count];
            for (var i = 0; i < players.Length; ++i)
            {
                var (relativePosition, color) = Player.SnakeBlueprints[i];
                var absolutePosition = new PointF(relativePosition.X * board.Width, relativePosition.Y * board.Height);
                players[i] = new Player(Point.Truncate(absolutePosition), color);
            }

            return players;
        }

        private void TimerEvent(object sender, EventArgs e)
        {
            _main.BackColor = System.Drawing.Color.Violet;

            foreach (var snake in _players.Select(p => p.Snake))
            {
                snake.Step();
            }

            TryCollide();

            _main.Refresh();
        }

        private void Draw(Object obj, PaintEventArgs e)
        {
            _renderer.Clear();

            var drawables = new List<IDrawable>();
            drawables.AddRange(foods);
            drawables.AddRange(_players.Select(player => player.Snake));

            foreach (var drawable in drawables)
            {
                drawable.Draw(_renderer);
            }

            _renderer.Display((Control)obj, e.Graphics);
        }

        // Checks each collidable for collisions and runs collision method if true
        private void TryCollide()
        {
            // Add all ICollidables (food and each player's snake) to the same list for easy iteration.
            var collidables = new List<ICollidable>();
            collidables.AddRange(foods);
            collidables.AddRange(_players);

            foreach (var player in _players)
            {
                // If collidable collides
                foreach (var collidable in collidables.Where(collidable => collidable.CheckCollision(player.Snake)))
                {
                    collidable.OnCollision(player);
                }
            }

            foods.RemoveAll(food => !food.IsActive);
        }
    }
}
