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
        armorUpgrade =  new Upgrades([10, 20, 50, 70, 100]);
        capacityUpgrade =  new Upgrades([30, 50, 70, 120, 200]);
        panel = new Panel(new Sprite("Panel1"), 4, 4);
        panel.SetTitle("Upgrades Shop");
        panel.AddButton(4,"Drill",()=>ApplyUpgrade(drillUpgrade,player.UpgradeDrill),180,60);
        panel.AddButton(5,"Fuel",()=>ApplyUpgrade(fuelUpgrade,player.UpgradeFuel),180,60);
        panel.AddButton(6,"Capacity",()=>ApplyUpgrade(capacityUpgrade,player.Inventory.UpgradeCapacity),180,60);
        panel.AddButton(7,"Armor",()=>ApplyUpgrade(armorUpgrade,player.UpgradeArmor),180,60);
        panel.AddCloseButton(2,CloseShop);
    }
    

    private void ApplyUpgrade(Upgrades upgrade, Action<float> applyToPlayer)
    {
        if (!upgrade.TryUpgrade(player)) return;
        applyToPlayer(upgrade.CurrentValue());
    }
    
    
}