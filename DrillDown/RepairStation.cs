using System;
using Microsoft.Xna.Framework;

namespace DrillDown;

public class RepairStation : Shop
{
    private float repairPrice = 10f;
    public RepairStation(string spriteName, float scale, float worldXPos, Player player) : base(spriteName, scale, worldXPos, player)
    {
        panel = new Panel(new Sprite("Panel"), 3, 3);
        panel.SetTitle("Repair Station");
        panel.AddButton(3,"50$",() => Repair(50));
        panel.AddButton(4,"100$",() => Repair(100));
        panel.AddButton(5,"200$",() => Repair(200));
        panel.AddButton(7,"Full Repair",() => FullRepair());
        panel.AddCloseButton(2,CloseShop);
    }

    private void Repair(int amount)
    {
        float repairNeeded = player.MaxHealth -  player.Health;
        if (repairNeeded <= 0)
        {
            Console.WriteLine("No Repair Needed!");
            return;
        }
        if (!player.TrySpendMoney(amount))
        {
            Console.WriteLine("Not enough Money!");
            return;
        }
        float repairBought = amount/repairPrice;
        Console.WriteLine("Repair Needed: " + repairNeeded);
        Console.WriteLine("Repair Bought: " + repairBought);
        player.Repair(repairBought);
    }

    private void FullRepair()
    {
        float repairAmount = player.MaxHealth -  player.Health;
        int totalRepairPrice = (int)(repairAmount * repairPrice);
        if (repairAmount <= 0)
        {
            Console.WriteLine("No Repair Needed!");
            return;
        }
        if (!player.TrySpendMoney(totalRepairPrice))
        {
            Console.WriteLine("Not enough Money!");
            return;
        }
        Console.WriteLine("Repair: " + repairAmount);
        player.Repair(repairAmount);
    }
}