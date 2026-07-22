using System;
using Microsoft.Xna.Framework.Input;

namespace DrillDown;

public class GasStation : Shop
{
    private float gasPrice = 0.7f;
    
    public GasStation(string spriteName, float scale, float worldXPos) : base(spriteName, scale, worldXPos)
    {
    }

    public override void Interact(Player player)
    {
        float missingFuel = player.MaxFuel -  player.Fuel;
        
    }

    /*public void Update(Player player)
    {
        if(IsPlayerInside(player.destRect) && Keyboard.GetState().IsKeyDown(Keys.E))
            Console.WriteLine("Entered GasStation");
    }*/
}