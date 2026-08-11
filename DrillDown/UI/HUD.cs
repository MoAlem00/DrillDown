using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class HUD : IDrawable,IUpdatable
{
    private Inventory inventory;
    private Sprite inventorySlots;
    private int columns = 5;
    private int rows = 3;
    private int inventoryWidth;
    private int inventoryHeight;
    private int margin = 60;
    private Vector2 inventorySlotsPos;
    private Text counterText;
    private int iconPaddingX = 16;
    private int iconPaddingY = 2;
    private int textPaddingX = 32;
    private int textPaddingY = 45;
    private Text moneyText;
    private int slotSize = 64;
    private Text capacity;
    private Text warningText;
    private Bar fuelBar;
    private Sprite fuelIcon;
    private Bar healthBar;
    private Sprite healthIcon;
    private Sprite bar,barFill;
    private float totalTime;
    private float pulseTimer;
    private float effectTime = 2f;
    private float currentTime;
    private bool isInventoryFull;
    

    public HUD(Inventory inventory)
    {
        fuelIcon = new Sprite("OilIcon");
        healthIcon = new Sprite("HealthIcon");
        bar = new Sprite("Bar");
        barFill = new Sprite("BarFill");
        healthBar = new Bar(bar, barFill,healthIcon,new Vector2(margin,margin*2.2f),Color.Red,Color.DarkRed);
        fuelBar = new Bar(bar, barFill,fuelIcon,new Vector2(margin,margin),Color.Yellow,Color.DarkGoldenrod);
        counterText = Text.CreateDefault();
        moneyText = Text.CreateDefault("$" + "0");
        capacity = Text.CreateDefault("0" + "Kg");
        warningText = Text.CreateDefault();
        moneyText.tm.position = new Vector2(Game1._screenTopCenter.X, 50f);
        counterText.tm.scale = new Vector2(0.7f, 0.7f);
        this.inventory = inventory;
        inventorySlots = new Sprite("InventorySlots");
        inventoryWidth = inventorySlots.texture.Width;
        inventoryHeight = inventorySlots.texture.Height;
        inventorySlots.anchor = Anchor.TopLeft;
        inventorySlotsPos = new Vector2(Game1._screenWidth - inventoryWidth - margin, margin);
        inventorySlots.tm.position = inventorySlotsPos;
        capacity.tm.position = new Vector2(inventorySlotsPos.X + inventoryWidth/2f, inventorySlotsPos.Y + inventoryHeight + 30);
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        inventorySlots.Draw(spriteBatch);
        int slot = 0;
        foreach (var material in inventory.Materials)
        {
            int col = slot % columns;
            int row = slot / columns;
            Vector2 slotPos = inventorySlotsPos + new Vector2(col * slotSize, row * slotSize);
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
        capacity.DrawTextBackground(spriteBatch);
        capacity.Draw(spriteBatch);
        fuelBar.Draw(spriteBatch);
        healthBar.Draw(spriteBatch);
        moneyText.Draw(spriteBatch);
        warningText.Draw(spriteBatch);
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

    public void HandleCapacityChange(float amount)
    {
        capacity.text = $"{amount:0.0}Kg/{inventory.Capacity}Kg";
    }

    public void HandleInventoryFull()
    {
        isInventoryFull = true;
        currentTime = 0f;
        pulseTimer = 0f;
    }
    
    public void HandleInventoryEmpty()
    {
        isInventoryFull = false;
        warningText.text = "";
    }

    public void Start()
    {
    }

    public void Update(GameTime gameTime)
    {
        if (isInventoryFull)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            currentTime += elapsed;
            if (currentTime < effectTime)
            {
                pulseTimer += elapsed * 10f;
                float scale = 1f + 0.1f *(float)Math.Sin(pulseTimer);
                warningText.text = "Inventory Is Full!!!";
                warningText.color = Color.Red;
                warningText.tm.position = Game1._screenCenter;
                warningText.tm.scale = new Vector2(scale, scale);
            }
            else
            {
                warningText.text = "";
                isInventoryFull = false;
            }
        }
    }
}