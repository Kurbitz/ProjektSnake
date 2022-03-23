using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace ProjectSnake
{
    internal class Engine
    {
        public static readonly int MaxPlayerCount =
            Math.Min(Player.SnakeBlueprints.Length, Controls.ControlsBlueprints.Length);

        private readonly List<Food> _foods = new List<Food>();
        public readonly Player[] Players;
        public Board Board;
        private readonly Random _rand = new Random();
        public bool IsPaused = false;
        public bool GameOver { get; private set; } = false;

        public Engine(int playerCount)
        {
            // Det måste finnas tillräckligt många blueprints för att kunna stödja alla möjliga spelarantal.
            // Om man vill stödja fler spelare får man lägga till flera blueprints.
            Debug.Assert(Player.SnakeBlueprints.Length >= MaxPlayerCount);
            Debug.Assert(Controls.ControlsBlueprints.Length >= MaxPlayerCount);

            Board.Width = 40;
            Board.Height = 30;

            Players = InitializePlayers(playerCount);

            AddFood(FoodTypes.Standard, GetFreePosition());
        }

        private Player[] InitializePlayers(int count)
        {
            Debug.Assert(count <= Player.SnakeBlueprints.Length);

            var players = new Player[count];
            for (var i = 0; i < players.Length; ++i)
            {
                var (relativePosition, direction, color) = Player.SnakeBlueprints[i];
                var absolutePosition = new PointF(relativePosition.X * Board.Width, relativePosition.Y * Board.Height);
                players[i] = new Player(Point.Truncate(absolutePosition), direction, color);
                players[i].Controls = Controls.ControlsBlueprints[i];
            }

            return players;
        }

        public void Tick()
        {
            if (IsPaused)
            {
                return;
            }

            foreach (var snake in Players.Select(p => p.Snake))
            {
                snake.Step();
            }

            TryCollide();

            SpawnFood();
        }

        public void Draw(IRenderer renderer)
        {
            var drawables = new List<IDrawable>();
            drawables.AddRange(_foods);
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
            collidables.AddRange(_foods);
            collidables.AddRange(Players);

            if (Players.All(player => !player.Snake.IsAlive))
            {
                GameOver = true;
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

            _foods.RemoveAll(food => !food.IsActive);
            foreach (var snake in Players.Select(player => player.Snake))
            {
                if (!snake.IsAlive)
                {
                    snake.Clear();
                }
            }
        }

        private enum FoodTypes
        {
            Standard,
            Valuable,
            Diet
        }

        private void SpawnFood()
        {
            // Se till att det finns minst en mat på brädet
            if (_foods.Count > 0)
            {
                // Styr spawnrate och max antal mat på brädet
                if (_foods.Count >= 3 || _rand.Next(1, 100) > 2)
                {
                    return;
                }
            }

            var randomFood = GetRandomFood();

            // Se till att det inte finns för många DietFood på brädet
            if (randomFood == FoodTypes.Diet)
            {
                if (_foods.Count(f => f.GetType() == typeof(DietFood)) > 1)
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

            foreach (var food in _foods)
            {
                freeSegments.RemoveAll(p => p == food.Position);
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
                    _foods.Add(new StandardFood(position));
                    break;
                case FoodTypes.Valuable:
                    _foods.Add(new ValuableFood(position));
                    break;
                case FoodTypes.Diet:
                    _foods.Add(new DietFood(position));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ClearBoard() => _foods.Clear();
    }
}
