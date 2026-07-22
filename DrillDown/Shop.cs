using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DrillDown;

public abstract class Shop : IDrawable
{
    private Sprite sprite;
    private Rectangle bounds;
    private int entranceWidth = 80;
    private int entranceHeight = 60;
    protected bool isOpen;
    private bool wasEPressed;
    private Sprite panel;
    public Rectangle Bounds => bounds;

    protected Shop(string spriteName, float scale, float worldXPos)
    {
        sprite = new Sprite(spriteName);
        sprite.BottomLeftOrigin();
        sprite.tm.scale = new Vector2(scale, scale);
        sprite.tm.position = new Vector2(Game1.groundLevel.X + worldXPos * Game1.blockSize, Game1.groundLevel.Y);
        sprite.sortingOrder = 0.8f;
        float shopXPos = sprite.tm.position.X;
        int scaledWidth = (int)(sprite.texture.Width * scale);
        bounds = new Rectangle(
            (int)(shopXPos + scaledWidth * 0.5f - entranceWidth * 0.5f),
            (int)(Game1.groundLevel.Y - entranceHeight),
            entranceWidth,
            entranceHeight
        );
        panel = new Sprite("Panel");
        panel.CenterOrigin();
        panel.tm.scale = new Vector2(3.5f, 3.5f);
        panel.tm.position = Game1._screenCenter;
        panel.sortingOrder = 1f;
    }
    
    public abstract void Interact(Player player);
    public void Draw(SpriteBatch spriteBatch)
    {
        sprite.Draw(spriteBatch);
    }

    public void UpdatePanel(Player player)
    {
        bool ePressed = Keyboard.GetState().IsKeyDown(Keys.E);
        if (IsPlayerInside(player.destRect) && ePressed && !wasEPressed)
            isOpen = true;
        
        if(Keyboard.GetState().IsKeyDown(Keys.F))
            isOpen = false;
        
        wasEPressed = ePressed;
    }
    
    public void DrawPanel(SpriteBatch spriteBatch)
    {
        if (!isOpen) return;
        panel.Draw(spriteBatch);
    }

    

    public bool IsPlayerInside(Rectangle playerRect)
    {
        return playerRect.Intersects(bounds);
    }
}