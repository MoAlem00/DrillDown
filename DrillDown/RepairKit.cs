namespace DrillDown;

public class RepairKit : Item
{
    public RepairKit(ItemType type, Sprite icon, int cost) : base(type, icon, cost,cooldown:1.2f)
    {
    }

    public override void Use(Player player)
    {
        player.Repair(player.MaxHealth/3f);
    }
}