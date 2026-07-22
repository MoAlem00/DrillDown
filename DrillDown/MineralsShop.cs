using Microsoft.Xna.Framework;

namespace DrillDown;

public class MineralsShop : Shop
{
    public MineralsShop(string spriteName, float scale, float worldXPos) : base(spriteName, scale, worldXPos)
    {
    }

    public override void Interact(Player player)
    {
        throw new System.NotImplementedException();
    }
}