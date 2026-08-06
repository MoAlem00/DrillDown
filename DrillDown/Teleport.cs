using System;
using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class Teleport : Item
{
    public Teleport(ItemType type, Sprite icon, int cost) : base(type, icon, cost)
    {
        type = ItemType.Teleport;
    }

    public override void Use(Player player)
    {
        player.Teleport();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }
}