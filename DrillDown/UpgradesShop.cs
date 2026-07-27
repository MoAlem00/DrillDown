namespace DrillDown;

public class UpgradesShop : Shop
{
    private Upgrades drillUpgrade;
    private Upgrades fuelUpgrade;
    private Upgrades capacityUpgrade;
    private Upgrades armorUpgrade;
    
    protected UpgradesShop(string spriteName, float scale, float worldXPos, Player player) : base(spriteName, scale, worldXPos, player)
    {
        
    }
}