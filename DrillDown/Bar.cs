using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class Bar
{
    public Texture2D background {get; set;}
    private Texture2D fill {get; set;}
    private Texture2D icon {get; set;}
    private Vector2 position;
    private Color fillColor,bgColor;
    private Vector2 fillOffset = new Vector2(1f, 1f);
    private int fillMaxWidth = 230;
    private int fillMaxHeight = 20;
    private float ratio;

    public Bar(Texture2D background, Texture2D fill, Texture2D icon, Vector2 position, Color fillColor, Color bgColor)
    {
        this.background = background;
        this.fill = fill;
        this.icon = icon;
        this.position = position;
        this.fillColor = fillColor;
        this.bgColor = bgColor;
    }
    
    public void SetRatio(float ratio) => this.ratio = Math.Clamp(ratio, 0f, 1f);

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(icon, position, Color.White);
        spriteBatch.Draw(background, new Vector2(position.X + icon.Width,position.Y + (icon.Height - background.Height)*0.5f), bgColor);
        Rectangle fillRect = new Rectangle(
            (int)(position.X + icon.Width),
            (int)(position.Y + (icon.Height - background.Height)*0.5f),
            (int)(fillMaxWidth * ratio),
            fillMaxHeight);
        spriteBatch.Draw(fill, fillRect, fillColor);
    }
}