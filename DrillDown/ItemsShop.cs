using System;
using System.Collections.Generic;

namespace DrillDown;

public class ItemsShop : Shop
{
    private Dictionary<ItemType, Item> items = new();
    public ItemsShop(string spriteName, float scale, float worldXPos, Player player) : base(spriteName, scale, worldXPos, player)
    {
        Item teleport = new Teleport(ItemType.Teleport, new Sprite("Teleport"), 1000);
        items.Add(ItemType.Teleport, teleport);
        panel = new Panel(new Sprite("Panel1"), 4, 4);
        panel.SetTitle("Items Shop");
        panel.AddSpriteButton(4,"Teleport",() => BuyItem(teleport),new Sprite("Teleport"),100,75);
        panel.AddCloseButton(3,CloseShop);
    }

    private void BuyItem(Item item)
    {
        if (!player.TryBuyItem(item))
        {
            Console.WriteLine($"{item.Type} Buy Failed");
            return;
        } 
        Console.WriteLine($"{item.Type} Bought {item.Cost}");
    }
}