using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace DrillDown;

public class Text : IUpdatable, IDrawable
{
    public Transform tm = new Transform();
    public SpriteFont font;
    public Color color = Color.White;
    public float sortingOrder = 0;
    public SpriteEffects effects = SpriteEffects.None;
    public string text = string.Empty;
    public bool centered = true;
    
    
    public virtual void Start()
    {
       
    }

    public virtual void Update(GameTime gameTime)
    {
        
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (font == null || text == null) return;
        Vector2 origin = centered ? font.MeasureString(text) * 0.5f : Vector2.Zero;
        
        spriteBatch.DrawString(
            font,
            text,
            tm.position, 
            color,
            MathHelper.ToRadians(tm.rotation),
            origin,
            tm.scale,
            effects,
            sortingOrder
        );
    }
    
    public Rectangle GetTextBackgroundRect(int padding = 5)
    {
        Vector2 size = font.MeasureString(text) * tm.scale;

        return new Rectangle(
            (int)(tm.position.X - size.X * 0.5f - padding),
            (int)(tm.position.Y - size.Y * 0.5f - padding),
            (int)size.X + padding * 2,
            (int)size.Y + padding * 2
        );
    }

    public void DrawTextBackground(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Button.Pixel, GetTextBackgroundRect(), null, Color.Black * 0.7f,
            0f, Vector2.Zero, SpriteEffects.None, sortingOrder - 0.01f);
    }
    
    public static Text CreateDefault(string content = "")
    {
        return new Text
        {
            text = content,
            font = Game1._font,
            color = Color.White,
            sortingOrder = 1f
        };
    }

    /*public static Text CreateCustom(Color color,float scale)
    {
        return new Text
        {
            font = Game1._font,
            color = Color.White,
            sortingOrder = 1f,
        };
    }*/
}