using System;

namespace DrillDown;

public class Bomb : Item
{
    public Bomb(ItemType type, Sprite icon, int cost) : base(type, icon, cost)
    {
        type = ItemType.Bomb;
    }

    public override void Use(Player player)
    {
        player.world.DestroyArea(player.tm.position,1);
    }
}