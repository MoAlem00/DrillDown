using System;
namespace DrillDown;

public class GasStation : Shop
{
    private float gasPrice = 0.7f;


    public GasStation(string spriteName, float scale, float worldXPos, Player player) : base(spriteName, scale, worldXPos, player)
    {
        panel = new Panel(new Sprite("Panel"), 3, 3);
        panel.SetTitle("Gas Station");
        panel.AddButton(3,"20$", () => BuyFuel(20));
        panel.AddButton(4,"50$",() => BuyFuel(50));
        panel.AddButton(5,"100$",() => BuyFuel(100));
        panel.AddButton(7,"Full Tank",() => BuyFullTank());
        panel.AddCloseButton(2,CloseShop);
    }
    

    private void BuyFuel(int amount)
    {
        float missingFuel = player.MaxFuel -  player.Fuel;
        if (missingFuel <= 0)
        {
            Console.WriteLine("Fuel Is Full!");
            return;
        }
        if (!player.TrySpendMoney(amount))
        {
            Console.WriteLine("Not enough Money!");
            return;
        }
        float fuelBought = amount/gasPrice;
        player.Refuel(fuelBought);
    }

    private void BuyFullTank()
    {
        float missingFuel = player.MaxFuel -  player.Fuel;
        int totalFuelPrice = (int)(missingFuel * gasPrice);
        if (missingFuel <= 0)
        {
            Console.WriteLine("Fuel Is Full!");
            return;
        }
        if (!player.TrySpendMoney(totalFuelPrice))
        {
            Console.WriteLine("Not enough Money!");
            return;
        }
        player.Refuel(missingFuel);
    }
    
}