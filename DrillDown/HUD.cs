using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class HUD : IDrawable
{
    private Inventory inventory;
    private Sprite inventorySlots;
    private int columns = 5;
    private int rows = 3;
    private int imageWidth;
    private int margin = 50;
    private Vector2 inventorySlotsOrigin;
    private Text counterText;
    private int iconPaddingX = 16;
    private int iconPaddingY = 2;
    private int textPaddingX = 32;
    private int textPaddingY = 45;
    private Text moneyText;
    private int slotSize = 64;
    private Text capacity;
    
    private Bar fuelBar;
    private Sprite fuelIcon;
    
    private Bar healthBar;
    private Sprite healthIcon;

    public HUD(Inventory inventory, SpriteFont font)
    {
        fuelIcon = new Sprite("FuelIcon");
        healthIcon = new Sprite("HealthIcon");
        healthIcon.tm.scale = new Vector2(1.2f, 1.2f);
        healthBar = new Bar(SpriteManager.GetSprite("Bar").texture,
            SpriteManager.GetSprite("BarFill").texture,healthIcon.texture,new Vector2(margin,margin*2.2f),Color.Red,Color.DarkRed);
        fuelBar = new Bar(SpriteManager.GetSprite("Bar").texture,
            SpriteManager.GetSprite("BarFill").texture,fuelIcon.texture,new Vector2(margin,margin),Color.Yellow,Color.DarkGoldenrod);
        counterText = Text.CreateDefault();
        moneyText = Text.CreateDefault("$" + "0");
        capacity = Text.CreateDefault("0" + "Kg");
        moneyText.tm.position = new Vector2(Game1._screenTopCenter.X, 50f);
        counterText.tm.scale = new Vector2(0.7f, 0.7f);
        this.inventory = inventory;
        inventorySlots = new Sprite("InventorySlots");
        imageWidth = inventorySlots.texture.Width;
        inventorySlots.anchor = Anchor.TopLeft;
        inventorySlotsOrigin = new Vector2(Game1._screenWidth - imageWidth - margin, margin);
        capacity.tm.position = new Vector2(Game1._screenRightCorner.X - imageWidth - 100, 50f);
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(inventorySlots.texture,inventorySlotsOrigin,Color.White);
        int slot = 0;
        foreach (var material in inventory.Materials)
        {
            int col = slot % columns;
            int row = slot / columns;
            Vector2 slotPos = inventorySlotsOrigin + new Vector2(col * slotSize, row * slotSize);
            Rectangle iconRect = new Rectangle((int)slotPos.X + iconPaddingX, (int)slotPos.Y + iconPaddingY,
                32, 32);
            spriteBatch.Draw(material.Key.Texture,iconRect,Color.White);
            Vector2 textPos = slotPos + new Vector2(textPaddingX, textPaddingY);
            counterText.text = material.Value.ToString();
            counterText.tm.position = textPos;
            counterText.Draw(spriteBatch);
            slot++;
        }
        
        moneyText.DrawTextBackground(spriteBatch);
        capacity.Draw(spriteBatch);
        fuelBar.Draw(spriteBatch);
        healthBar.Draw(spriteBatch);
        moneyText.Draw(spriteBatch);
    }

    public void HandleFuelChange(float ratio)
    {
        fuelBar.SetRatio(ratio);
    }

    public void HandleHealthChange(float ratio)
    {
        healthBar.SetRatio(ratio);
    }

    public void HandleMoneyChange(int amount)
    {
        moneyText.text = "$" + amount;
    }

    public void HandleCapacityChange(int amount)
    {
        capacity.text = amount + "Kg" + "/" + inventory.Capacity + "Kg";
    }
    
}