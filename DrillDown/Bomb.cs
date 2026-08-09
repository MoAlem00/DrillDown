using System;
using Microsoft.Xna.Framework;

namespace DrillDown;

public class Bomb : Item
{
    
    public Bomb(ItemType type, Sprite icon, int cost) : base(type, icon, cost,cooldown:0.9f)
    {
    }

    public override void Use(Player player)
    {
        player.world.DestroyArea(player.tm.position,1);
        OnItemUse("BombEffect",player.tm.position);
    }
}