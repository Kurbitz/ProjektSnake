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
        private MainForm _main;
        private WinFormsRenderer _renderer;
        private Timer _timer = new Timer();
        private List<Food> foods = new List<Food>();
        public Player[] Players;
        public Board Board;
        private Random _rand = new Random();

        private ScoreLabel[] _scoreLabels;

        public void Run(MainForm mainForm)
        {
            _main = mainForm;
            Board.Width = 40;
            Board.Height = 30;

            _renderer = new WinFormsRenderer(Board);

            var playerCount = 2;
            Players = InitializePlayers(playerCount);
            _scoreLabels = InitializeScoreLabels(Players);

            foreach (var label in _scoreLabels)
            {
                _main.Controls.Add(label);
            }

            AddFood(FoodTypes.Standard, GetFreePosition());
            _main.KeyDown += MainOnKeyDown;
            _main.Paint += Draw;
            _timer.Tick += TimerEvent;
            _timer.Interval = 1000 / 60;
            _timer.Start();
        }

        private void MainOnKeyDown(object sender, KeyEventArgs e)
        {
            var key = e.KeyCode;
            foreach (var player in Players)
            {
                var dirction = player.controls.todirection(key);
                if (dirction == null)
                {
                    continue;
                }
                player.Snake.Move((Direction)dirction);
            }
           
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
                var absolutePosition = new PointF(relativePosition.X * Board.Width, relativePosition.Y * Board.Height);
                players[i] = new Player(Point.Truncate(absolutePosition), color);
                players[i].controls = Controls.controlsBluprints[i];
            }

            return players;
        }

        private void TimerEvent(object sender, EventArgs e)
        {
            foreach (var snake in Players.Select(p => p.Snake))
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

            Draw(_renderer);

            foreach (var label in _scoreLabels)
            {
                _renderer.Draw(label);
            }

            _renderer.Display((Control)obj, e.Graphics);
        }

        public void Draw(IRenderer renderer)
        {
            var drawables = new List<IDrawable>();
            drawables.AddRange(foods);
            drawables.AddRange(Players);

            foreach (var drawable in drawables)
            {
                drawable.Draw(renderer);
            }
        }

        // Checks each collidable for collisions and runs collision method if true
        private void TryCollide()
        {
            // Add all ICollidables (food and each player's snake) to the same list for easy iteration.
            var collidables = new List<ICollidable>();
            collidables.AddRange(foods);
            collidables.AddRange(Players);

            if (Players.All(player => !player.Snake.IsAlive))
            {
                GameOver();
            }

            foreach (var player in Players)
            {
                // Om Out of Bounds
                if (player.CheckCollision(Board))
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
            foreach (var snake in Players.Select(player => player.Snake))
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

            var randomFood = GetRandomFood();

            // Se till att det inte finns för många DietFood på brädet
            if (randomFood == FoodTypes.Diet)
            {
                if (foods.Count(f => f.GetType() == typeof(DietFood)) > 1)
                {
                    return;
                }
            }

            var foodPosition = GetFreePosition();
            AddFood(randomFood, foodPosition);
        }

        private FoodTypes GetRandomFood()
        {
            // generera en random mattyp och position
            var foodTypes = Enum.GetValues(typeof(FoodTypes));
            var randomFood = (FoodTypes)foodTypes.GetValue(_rand.Next(foodTypes.Length));
            return randomFood;
        }

        private Point GetFreePosition()
        {
            // Gör en lista med alla möjliga platser på brädet
            var freeSegments = (from x in Enumerable.Range(0, Board.Width)
                from y in Enumerable.Range(0, Board.Height)
                select new Point(x, y)).ToList();

            // Ta bort alla upptagna positioner, där det finns ormar eller mat
            foreach (var player in Players)
            {
                freeSegments.RemoveAll(p => player.Snake.CheckCollision(p));
            }

            foreach (var food in foods)
            {
                freeSegments.RemoveAll(p => p == food.position);
            }

            return freeSegments[_rand.Next(freeSegments.Count)];
        }

        private void AddFood(FoodTypes type, Point position)
        {
            if (position.X < 0 || position.Y < 0 || position.X >= Board.Width || position.Y >= Board.Height)
            {
                throw new ArgumentOutOfRangeException();
            }

            switch (type)
            {
                case FoodTypes.Standard:
                    foods.Add(new StandardFood(position));
                    break;
                case FoodTypes.Valuable:
                    foods.Add(new ValuableFood(position));
                    break;
                case FoodTypes.Diet:
                    foods.Add(new DietFood(position));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
