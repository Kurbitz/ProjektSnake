using System.Drawing;
using System.Windows.Forms;

namespace ProjectSnake
{
    public class ScoreLabel : Label
    {
        private readonly Player _player;

        public ScoreLabel(Player player)
        {
            _player = player;
            ForeColor = player.Snake.Color;
            BackColor = Color.Transparent;
        }

        public void UpdateText()
        {
            Text = $"Score: {_player.Score}";
        }
    }
}
