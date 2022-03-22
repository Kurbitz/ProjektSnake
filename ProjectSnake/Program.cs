using System;
using System.Windows.Forms;

namespace ProjectSnake
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            var mainForm = new MainForm();
            Application.Run(mainForm);
        }
    }
}
