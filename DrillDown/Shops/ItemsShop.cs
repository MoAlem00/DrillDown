using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class ItemsShop : Shop
{
    private class ItemEntry
    {
        public ItemType type;
        public Text costText,quantityText;
        public int slot;

        public ItemEntry(ItemType type,int slot)
        {
            this.type = type;
            costText = Text.CreateDefault();
            quantityText = Text.CreateDefault();
            costText.tm.scale = new Vector2(0.7f, 0.7f);
            this.slot = slot;
        }
    }

    private List<ItemEntry> entries = new();

    
    public ItemsShop(string spriteName, float scale, float worldXPos, Player player) : base(spriteName, scale, worldXPos, player)
    {
        panel = new Panel(new Sprite("Panel1"), 4, 4);
        panel.SetTitle("Items Shop");
        AddItem(ItemType.Teleport,4,"Teleport","Teleport",100,75);
        AddItem(ItemType.Bomb,5,"Bomb","Bomb",130,70);
        AddItem(ItemType.FuelTank,6,"Fuel","FuelTank",80,80);
        AddItem(ItemType.RepairKit,7,"Repair","RepairKit",90,100);
        panel.AddCloseButton(3,CloseShop);
    }

    private void BuyItem(ItemType type)
    {
        Item item = player.Inventory.GetItem(type);
        if (item == null) return;
        if (player.TrySpendMoney(item.Cost))
            item.AddQuantity(1);
    }
    
    private void AddItem(ItemType type,int slot, string label, string icon,int width = 80, int height = 80)
    {
        ItemEntry entry = new ItemEntry(type,slot);
        entries.Add(entry);
        panel.AddSpriteButton(slot, label, () => BuyItem(entry.type), new Sprite(icon), width, height);
    }
    
    public override void DrawPanel(SpriteBatch spriteBatch)
    {
        base.DrawPanel(spriteBatch);
        if (!isOpen) return;
        foreach (var entry in entries)
        {
            Item item = player.Inventory.GetItem(entry.type);
            if (item == null) continue;
            entry.costText.text = $"Cost: ${item.Cost}";
            entry.costText.tm.position = panel.GetSlotUpperCenter(entry.slot + 4);
            entry.quantityText.text = $"x {item.Quantity}";
            entry.quantityText.tm.position = panel.GetSlotCenter(entry.slot + 4);
            entry.costText.Draw(spriteBatch);
            entry.quantityText.Draw(spriteBatch);
        }
    }
}