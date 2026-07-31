using System;

namespace DrillDown;

public class Upgrades
{
    public int[] upgradesCosts = [5000, 10000, 20000, 50000, 100000];
    public int level = 0;
    private float[] upgradesValues;
    
    public Upgrades(float[] upgradesValues)
    {
        this.upgradesValues = upgradesValues;
    }
    
    public float CurrentValue() => upgradesValues[level-1];
    public bool IsMaxed => level >= upgradesCosts.Length;
    public int NextCost => IsMaxed ? -1 : upgradesCosts[level];

    public bool TryUpgrade(Player player)
    {
        if (IsMaxed)
        {
            Console.WriteLine("Maxed Out");
            return false;
        }

        if (!player.TrySpendMoney(upgradesCosts[level]))
        {
            Console.WriteLine("Not enough money");
            return false;
        }
        level++;
        return true;
    }


}