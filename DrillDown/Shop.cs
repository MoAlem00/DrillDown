using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DrillDown;

public class Shop : IDrawable
{
    protected Player player;
    private Sprite shopSprite;
    private Rectangle entranceBounds;
    private int entranceWidth = 80;
    private int entranceHeight = 60;
    protected bool isOpen;
    private bool wasEPressed;
    private bool playerIsInside;
    protected Panel panel;
    private Text promptText;
    private float promptOffsetY = -50f;
    private float promptOffsetX = 40f;
    public Rectangle EntranceBounds => entranceBounds;

    protected Shop(string spriteName, float scale, float worldXPos, Player player)
    {
        this.player = player;
        shopSprite = new Sprite(spriteName);
        shopSprite.BottomLeftOrigin();
        shopSprite.tm.scale = new Vector2(scale, scale);
        shopSprite.tm.position = new Vector2(Game1.groundLevel.X + worldXPos * Game1.blockSize, Game1.groundLevel.Y);
        shopSprite.sortingOrder = 0.8f;
        SetShopEntranceBounds(scale);
        SetPromptText();
    }
    
    
    private void SetShopEntranceBounds(float scale)
    {
        float shopXPos = shopSprite.tm.position.X;
        int scaledWidth = (int)(shopSprite.texture.Width * scale);
        entranceBounds = new Rectangle(
            (int)(shopXPos + scaledWidth * 0.5f - entranceWidth * 0.5f),
            (int)(Game1.groundLevel.Y - entranceHeight),
            entranceWidth,
            entranceHeight
        );
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        shopSprite.Draw(spriteBatch);
    }

    public void Update()
    {
        bool ePressed = Keyboard.GetState().IsKeyDown(Keys.E);
        playerIsInside = IsPlayerInside();
            
        if (playerIsInside && ePressed && !wasEPressed)
            isOpen = true;
        if(Keyboard.GetState().IsKeyDown(Keys.F))
            isOpen = false;
        
        wasEPressed = ePressed;
    }

    public void UpdatePanel(GameTime gameTime)
    {
        if (panel == null) return;
        if (!isOpen) return;
        panel.UpdatePanel(gameTime);
    }
    
    public virtual void DrawPanel(SpriteBatch spriteBatch)
    {
        if (panel == null) return;
        if (!isOpen) return;
        panel.DrawPanel(spriteBatch);
    }

    public void DrawPrompt(SpriteBatch spriteBatch)
    {
        if (!playerIsInside || isOpen) return;
        promptText.DrawTextBackground(spriteBatch);
        promptText.Draw(spriteBatch);
    }

    private void SetPromptText()
    {
        promptText = new Text
        {
            text = "Press E to Enter",
            font = Game1._font,
            color = Color.White,
            sortingOrder = 1f
        };
        promptText.tm.position = new Vector2(entranceBounds.X + promptOffsetX, entranceBounds.Y + promptOffsetY);
    }

    private bool IsPlayerInside()
    {
        return player.destRect.Intersects(entranceBounds);
    }

    public void CloseShop()
    {
        isOpen = false;
    }
    
}