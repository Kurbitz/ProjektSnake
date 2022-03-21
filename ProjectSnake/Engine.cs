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
        private Random _rand = new Random();

        private ScoreLabel[] _scoreLabels;

        public void Run()
        {
            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            board.Width = 40;
            board.Height = 30;


            _renderer = new WinFormsRenderer(board);

            var playerCount = 2;
            _players = InitializePlayers(playerCount);
            _scoreLabels = InitializeScoreLabels(_players);

            foreach (var label in _scoreLabels)
            {
                _main.Controls.Add(label);
            }

            AddRandomFood(); 
            _main.Paint += Draw;
            _timer.Tick += TimerEvent;
            _timer.Interval = 1000 / 60;
            _timer.Start();
            Application.Run(_main);
        }

        private ScoreLabel[] InitializeScoreLabels(Player[] players)
        {
            var labels = new ScoreLabel[players.Length];
            for (var i = 0; i < labels.Length; ++i)
            {
                var label = new ScoreLabel(players[i]);
                label.Location = new Point(0, i * label.Height);
                labels[i] = label;
            }

            return labels;
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
            foreach (var snake in _players.Select(p => p.Snake))
            {
                snake.Step();
            }

            TryCollide();
            SpawnFood();
            _main.Refresh();
        }

        private void Draw(Object obj, PaintEventArgs e)
        {
            _renderer.Clear();

            var drawables = new List<IDrawable>();
            drawables.AddRange(foods);
            drawables.AddRange(_players);

            foreach (var drawable in drawables)
            {
                drawable.Draw(_renderer);
            }

            foreach (var label in _scoreLabels)
            {
                _renderer.Draw(label);
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

            if (_players.All(player => !player.Snake.IsAlive))
            {
                GameOver();
            }
            
            foreach (var player in _players)
            {
                // Om Out of Bounds
                if (player.CheckCollision(board))
                {
                    player.Snake.IsAlive = false;
                }

                // Om en player krockar med något som går att krocka med
                foreach (var collidable in collidables.Where(collidable => collidable.CheckCollision(player.Snake)))
                {
                    collidable.OnCollision(player);
                }
            }

            foods.RemoveAll(food => !food.IsActive);
            foreach (var snake in _players.Select(player => player.Snake))
            {
                if (!snake.IsAlive)
                {
                    snake.Clear();
                }
            }
        }

        private void GameOver()
        {
            throw new NotImplementedException();
        }

        enum FoodTypes
        {
            Standard,
            Valuable,
            Diet
        }

        private void SpawnFood()
        {
            // Styr spawnrate och max antal mat på brädet
            if (foods.Count >= 3 || _rand.Next(1, 100) > 2)
            {
                return;
            }

            AddRandomFood();
        }

        private void AddRandomFood()
        {
            // Gör en lista med alla möjliga platser på brädet
            var freeSegments = (from x in Enumerable.Range(0, board.Width)
                from y in Enumerable.Range(0, board.Height)
                select new Point(x, y)).ToList();

            // Ta bort alla upptagna positioner, där det finns ormar eller mat
            foreach (var player in _players)
            {
                freeSegments.RemoveAll(p => player.Snake.CheckCollision(p));
            }

            foreach (var food in foods)
            {
                freeSegments.RemoveAll(p => p == food.position);
            }

            // generera en random mattyp och position
            var foodTypes = Enum.GetValues(typeof(FoodTypes));
            var randomFood = (FoodTypes) foodTypes.GetValue(_rand.Next(foodTypes.Length));
            var pos = freeSegments[_rand.Next(freeSegments.Count)];

            switch (randomFood)
            {
                case FoodTypes.Standard:
                    foods.Add(new StandardFood(pos));
                    break;
                case FoodTypes.Valuable:
                    foods.Add(new ValuableFood(pos));
                    break;
                case FoodTypes.Diet:
                    foods.Add(new DietFood(pos));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
