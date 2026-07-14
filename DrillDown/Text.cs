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
    
    
    public virtual void Start()
    {
       
    }

    public virtual void Update(GameTime gameTime)
    {
        
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (font == null || text == null) return;
        Vector2 textCenter = font.MeasureString(text) * 0.5f;
        
        spriteBatch.DrawString(
            font,
            text,
            tm.position, 
            color,
            MathHelper.ToRadians(tm.rotation),
            textCenter,
            tm.scale,
            effects,
            sortingOrder
        );
    }
}