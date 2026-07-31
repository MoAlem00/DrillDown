
using System;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace DrillDown;

public class MineralsShop : Shop
{
    private Text totalValueText;
    private Text oresList;
    private Text emptyText;
    public MineralsShop(string spriteName, float scale, float worldXPos, Player player) : base(spriteName, scale, worldXPos, player)
    {
        panel = new Panel(new Sprite("Panel1"), 5, 5);
        panel.SetTitle("MineralsShop");
        panel.AddButton(22,"Sell All",() => SellAll());
        panel.AddCloseButton(4,CloseShop);
        totalValueText = new Text
        {
            font = Game1._font,
            color = Color.White,
            sortingOrder = 1f
        };
        oresList = new Text
        {
            font = Game1._font,
            color = Color.Black,
            sortingOrder = 1f,
            centered = false,
        };
        emptyText = new Text
        {
            font = Game1._font,
            color = Color.White,
            sortingOrder = 1f,
        };
        totalValueText.tm.position = panel.GetSlotCenter(17);
    }

    private void SellAll()
    {
        if (player == null) return;
        int totalValue = player.Inventory.GetOresTotalValue();
        player.AddMoney(totalValue);
        player.Inventory.ClearInventory();
    }
    
    public override void DrawPanel(SpriteBatch spriteBatch)
    {
        base.DrawPanel(spriteBatch);
        if (!isOpen) return;
        totalValueText.text = $"Total Price: $ {player.Inventory.GetOresTotalValue()}";
        int row = 0;
        int padding = 0;
        Vector2 startPos = panel.GetSlotUpperCenter(5);
        if (player.Inventory.Materials.Count == 0)
        {
            emptyText.text = "No Ores To Sell";
            emptyText.tm.position = panel.GetSlotCenter(7);
        }
        else
            emptyText.text = "";
        foreach (var material in player.Inventory.Materials)
        {
            oresList.tm.scale = new Vector2(0.6f, 0.6f);
            oresList.text = material.Key.Name + " x" + material.Value + " = $" + material.Key.SellCost * material.Value;
            if (row >= 10)
            {
                startPos = panel.GetSlotUpperCenter(7);
                row = 0;
                padding = 20;
            }
            oresList.tm.position = startPos + new Vector2(padding, row * 30);
            oresList.Draw(spriteBatch);
            row++;
        }
        emptyText.Draw(spriteBatch);
        totalValueText.DrawTextBackground(spriteBatch);
        totalValueText.Draw(spriteBatch);
    }
    
}