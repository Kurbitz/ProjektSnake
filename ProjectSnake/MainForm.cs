using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectSnake
{
    public partial class MainForm : Form
    {
        double aspectRatio = 0.75;
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
