using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class Bar
{
    public Sprite background;
    private Sprite fill;
    private Sprite icon;
    private Vector2 position;
    private Color fillColor,bgColor;
    private Vector2 fillOffset = new Vector2(1f, 1f);
    private int fillMaxWidth = 230;
    private int fillMaxHeight = 20;
    private float ratio;

    public Bar(Sprite background, Sprite fill, Sprite icon, Vector2 position, Color fillColor, Color bgColor)
    {
        this.background = background;
        this.fill = fill;
        this.icon = icon;
        this.position = position;
        icon.tm.position = position;
        this.fillColor = fillColor;
        this.bgColor = bgColor;
    }
    
    public void SetRatio(float ratio) => this.ratio = Math.Clamp(ratio, 0f, 1f);

    public void Draw(SpriteBatch spriteBatch)
    {
        icon.Draw(spriteBatch);
        spriteBatch.Draw(background.texture, new Vector2(position.X + 60,position.Y + (60 - background.texture.Height)*0.5f), bgColor);
        Rectangle fillRect = new Rectangle(
            (int)(position.X + 60),
            (int)(position.Y + (60 - background.texture.Height)*0.5f),
            (int)(fillMaxWidth * ratio),
            fillMaxHeight);
        spriteBatch.Draw(fill.texture, fillRect, fillColor);
    }
}