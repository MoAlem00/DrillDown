namespace DrillDown;

public class FuelTank : Item
{
    private float refuelAmount = 30f;
    public FuelTank(ItemType type, Sprite icon, int cost) : base(type, icon, cost)
    {
        type = ItemType.FuelTank;
    }

    public override void Use(Player player)
    {
        player.Refuel(refuelAmount);
    }
}