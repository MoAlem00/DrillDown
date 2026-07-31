using System;

namespace DrillDown;

public class UpgradesShop : Shop
{
    private Upgrades drillUpgrade;
    private Upgrades fuelUpgrade;
    private Upgrades capacityUpgrade;
    private Upgrades armorUpgrade;
    
    public UpgradesShop(string spriteName, float scale, float worldXPos, Player player) : base(spriteName, scale, worldXPos, player)
    {
        drillUpgrade = new Upgrades([0.08f, 0.15f, 0.3f, 0.5f, 1f]);
        fuelUpgrade =  new Upgrades([10, 20, 50, 70, 100]);
        panel = new Panel(new Sprite("Panel1"), 4, 4);
        panel.SetTitle("Upgrades Shop");
        panel.AddButton(4,"Drill",()=>UpgradeDrill(),180,60);
        panel.AddButton(5,"Fuel",()=>UpgradeFuel(),180,60);
        panel.AddCloseButton(2,CloseShop);
        /*panel.AddButton(3,"20$", () => BuyFuel(20));
        panel.AddButton(4,"50$",() => BuyFuel(50));
        panel.AddButton(5,"100$",() => BuyFuel(100));
        panel.AddButton(7,"Full Tank",() => BuyFullTank());
        panel.AddCloseButton(2,CloseShop);*/
    }

    private void UpgradeDrill()
    {
        if(!drillUpgrade.TryUpgrade(player)) return;
        Console.WriteLine($"Drill upgrade completed level:{drillUpgrade.level} cost:{drillUpgrade.upgradesCosts[drillUpgrade.level-1]}");
        player.UpgradeDrill(drillUpgrade.CurrentValue());
    }
    
    private void UpgradeFuel()
    {
        if(!fuelUpgrade.TryUpgrade(player)) return;
        Console.WriteLine($"Drill upgrade completed level:{fuelUpgrade.level} cost:{fuelUpgrade.upgradesCosts[fuelUpgrade.level-1]}");
        player.UpgradeFuel(fuelUpgrade.CurrentValue());
    }
}