﻿using System;
using System.Collections.Generic;
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
        private Player[] players;
        private Board board;

        public void Run()
        {
            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            board.Width = 40;
            board.Height = 30;

            foods.Add(new StandardFood(new Point(board.Width / 2, board.Height / 2)));

            _renderer = new WinFormsRenderer(board);

            _main.Paint += Draw;
            _timer.Tick += TimerEvent;
            _timer.Interval = 1000 / 60;
            _timer.Start();
            Application.Run(_main);
        }

        private void TimerEvent(object sender, EventArgs e)
        {
            _main.BackColor = System.Drawing.Color.Violet;
            TryCollide();
            _main.Refresh();
        }

        private void Draw(Object obj, PaintEventArgs e)
        {
            _renderer.Clear();

            var drawables = new List<IDrawable>();
            drawables.AddRange(foods);

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
            collidables.AddRange(players.Select(player => player.snake));

            foreach (var player in players)
            {
                // If collidable collides
                foreach (var collidable in collidables.Where(collidable => collidable.CheckCollision(player.snake)))
                {
                    collidable.OnCollision(player);
                }
            }
        }
    }
}
