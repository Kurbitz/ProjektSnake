using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProjectSnake
{
    public partial class MainForm : Form
    {
        private double aspectRatio = 0.75;

        private Engine _engine;
        private WinFormsRenderer _renderer;

        private Timer _timer = new Timer();

        public MainForm(int width = 800)
        {
            InitializeComponent();
            ClientSize = new Size(width, (int)(width * aspectRatio));
            BackColor = Color.FromArgb(29, 29, 29);
            DoubleBuffered = true;

            _engine = new Engine();
            _engine.Run(this);
            _renderer = _engine.Renderer;

            Paint += Draw;
            _timer.Tick += TimerEvent;
            _timer.Interval = 1000 / 60;
            _timer.Start();
        }

        private void Draw(Object obj, PaintEventArgs e)
        {
            _renderer.Clear();

            _engine.Draw(_renderer);

            foreach (var label in _engine._scoreLabels)
            {
                _renderer.Draw(label);
            }

            _renderer.Display((Control)obj, e.Graphics);
        }

        private void TimerEvent(object sender, EventArgs e)
        {
            _engine.Tick();

            Refresh();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            // NOTE(Johan): Om vi bara modifierar ClientSize.Height på detta sätt så går det bara
            // att ändra storleken genom att dra fönstret horisontellt, inte vertikalt.

            // ClientSize är storleken på fönstrets faktiska innehåll, utan title bar och liknande.
            ClientSize = new Size(ClientSize.Width, (int)(ClientSize.Width * aspectRatio));
        }
    }
}
