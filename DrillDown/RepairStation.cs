using Microsoft.Xna.Framework;

namespace DrillDown;

public class RepairStation : Shop
{
    public RepairStation(string spriteName, float scale, float worldXPos) : base(spriteName, scale, worldXPos)
    {
    }

    public override void Interact(Player player)
    {
        throw new System.NotImplementedException();
    }
}