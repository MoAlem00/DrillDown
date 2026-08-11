using System;
using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class Teleport : Item
{
    private string useSound = "portalEnter";
    public Teleport(ItemType type, Sprite icon, int cost) : base(type, icon, cost,cooldown:3f)
    {
    }

    public override bool Use(Player player)
    {
        Console.WriteLine("Teleporting");
        AudioManager.PlaySoundEffect(useSound,false,0.2f);
        player.Teleport();
        return true;
    }
}