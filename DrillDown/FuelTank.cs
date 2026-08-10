using System;
using Microsoft.Xna.Framework;

namespace DrillDown;

public class FuelTank : Item
{
    public FuelTank(ItemType type, Sprite icon, int cost) : base(type, icon, cost,cooldown:1.2f)
    {
    }

    public override bool Use(Player player)
    {
        if(Math.Abs(player.MaxFuel - player.Fuel) < 0.01f) return false;
        player.Refuel(player.MaxFuel/3f);
        AudioManager.PlaySoundEffect("RefuelSound");
        return true;
    }
}