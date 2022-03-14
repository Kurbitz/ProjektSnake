using System;
using System.Windows.Forms;

namespace ProjectSnake
{
    public partial class MainForm : Form
    {
        private double aspectRatio = 0.75;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            this.Height = (int)(this.Width * aspectRatio);
        }
    }
}
