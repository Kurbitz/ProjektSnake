using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectSnake
{
    class Engine
    {
        MainForm _main = new MainForm();
        Timer _timer = new Timer();
        List<Food> foods = new List<Food>();
        Player[] players;
        Board board;

        public void Run()
        {
            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            board.Width = 40;
            board.Height = 30;

            foods.Add(new StandardFood(new Point(board.Width / 2, board.Height / 2)));

            _main.Paint += Draw;
            _timer.Tick += TimerEvent;
            _timer.Interval = 1000 / 60;
            _timer.Start();
            Application.Run(_main);
        }

        private void TimerEvent(object sender, EventArgs e)
        {
            _main.BackColor = System.Drawing.Color.Violet;
        }

        double TileSize => _main.Width / board.Width;

        private void Draw(Object obj, PaintEventArgs e)
        {
            foreach (var food in foods)
            {
                e.Graphics.Clip = new Region(new Rectangle((int)(food.position.X * TileSize), (int)(food.position.Y * TileSize), (int)TileSize, (int)TileSize));
                food.Draw(e.Graphics);
            }
            //throw new NotImplementedException();
        }
    }
}
