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
        AudioManager.PlaySoundEffect("BombExplode",false,0.5f);
        OnItemUse("BombEffect",player.tm.position);
    }
}