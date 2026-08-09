using Microsoft.Xna.Framework;

namespace DrillDown;

public class FuelTank : Item
{
    private float refuelAmount = 30f;
    public FuelTank(ItemType type, Sprite icon, int cost) : base(type, icon, cost,cooldown:1.2f)
    {
    }

    public override void Use(Player player)
    {
        player.Refuel(refuelAmount);
    }
}