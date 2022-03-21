using System;

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
            var mainForm = new MainForm();
            var engine = new Engine();
            engine.Run(mainForm);
        }
    }
}
