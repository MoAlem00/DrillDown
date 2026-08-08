namespace DrillDown;

public class RepairKit : Item
{
    private float repairAmount = 40f;
    public RepairKit(ItemType type, Sprite icon, int cost) : base(type, icon, cost)
    {
        type = ItemType.RepairKit;
    }

    public override void Use(Player player)
    {
        player.Repair(repairAmount);
    }
}