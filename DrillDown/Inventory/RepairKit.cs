using System;

namespace DrillDown;

public class RepairKit : Item
{
    private string useSound = "UpgradeSound";
    public RepairKit(ItemType type, Sprite icon, int cost) : base(type, icon, cost,cooldown:1.2f)
    {
    }

    public override bool Use(Player player)
    {
        if (Math.Abs(player.Health - player.MaxHealth) < 0.01f) return false;
        player.Repair(player.MaxHealth/3f);
        AudioManager.PlaySoundEffect(useSound);
        return true;
    }
}