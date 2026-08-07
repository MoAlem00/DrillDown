using System;
using System.Collections.Generic;

namespace DrillDown;

public class ItemsShop : Shop
{
    public ItemsShop(string spriteName, float scale, float worldXPos, Player player) : base(spriteName, scale, worldXPos, player)
    {
        panel = new Panel(new Sprite("Panel1"), 4, 4);
        panel.SetTitle("Items Shop");
        panel.AddSpriteButton(4,"Teleport",() => BuyItem(ItemType.Teleport),new Sprite("Teleport"),100,75);
        panel.AddSpriteButton(5,"Bomb",() => BuyItem(ItemType.Bomb),new Sprite("Bomb"),120,70);
        panel.AddCloseButton(3,CloseShop);
    }

    private void BuyItem(ItemType type)
    {
        Item item = player.Inventory.GetItem(type);
        if (item == null) return;
        if (player.TrySpendMoney(item.Cost))
            item.AddQuantity(1);
    }
}