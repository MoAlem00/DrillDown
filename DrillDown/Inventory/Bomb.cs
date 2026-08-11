using System;
using Microsoft.Xna.Framework;

namespace DrillDown;

public class Bomb : Item
{
    private string bombSound = "BombExplode";
    private string bombEffect = "BombEffect";
    
    public Bomb(ItemType type, Sprite icon, int cost) : base(type, icon, cost,cooldown:0.9f)
    {
    }

    public override bool Use(Player player)
    {
        player.World.DestroyArea(player.tm.position,1);
        AudioManager.PlaySoundEffect(bombSound,false,0.5f);
        OnItemUse(bombEffect,player.tm.position);
        return true;
    }
}