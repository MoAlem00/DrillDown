using System;
using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class Teleport : Item
{
    public Teleport(ItemType type, Sprite icon, int cost) : base(type, icon, cost,cooldown:3f)
    {
    }

    public override void Use(Player player)
    {
        Console.WriteLine("Teleporting");
        player.Teleport();
    }
}