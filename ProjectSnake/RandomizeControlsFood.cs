using System.Drawing;

namespace ProjectSnake
{
    public class RandomizeControlsFood : Food
    {
        public RandomizeControlsFood(Point pos) : base(pos, Gruvbox.Purple, 10, 1)
        {
        }

        public override void OnCollision(Player player)
        {
            player.OnCollision(this);
            IsActive = false;
        }
    }
}
