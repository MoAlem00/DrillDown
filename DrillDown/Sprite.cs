using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class Sprite : IDrawable, IUpdatable
{
    public Transform tm = new Transform();
    public Texture2D texture;
    public SpriteSheet spriteSheet;
    public Color color = Color.White;
    public float sortingOrder = 0;
    public SpriteEffects effects = SpriteEffects.None;
    public Anchor anchor = Anchor.TopLeft;

    protected Rectangle? sourceRect = null;
    public Rectangle destRect;
    
    public Sprite(string spriteName)
    {
        spriteSheet = SpriteManager.GetSprite(spriteName);
        if (spriteSheet == null) return;
        texture = spriteSheet.texture;
        sourceRect = spriteSheet[0,0];
        UpdateDestRect();
    }
    
    
    public virtual void Start()
    {
    }
    

    public Vector2 GetOrigin()
    {
        if(sourceRect == null) return Vector2.Zero;
        return AnchorHelper.GetOrigin(anchor,new Vector2(sourceRect.Value.Width,sourceRect.Value.Height));
    }
    private void UpdateDestRect()
    {
        destRect = GetDestRect(sourceRect);
    }
    public virtual void Update(GameTime gameTime)
    {
        UpdateDestRect();
    }
    
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        destRect = GetDestRect(sourceRect);
        
        spriteBatch.Draw(
            texture, 
            tm.position,
            sourceRect,
            color,
            MathHelper.ToRadians(tm.rotation),
            GetOrigin(),
            tm.scale,
            effects,
            sortingOrder
        );
    }
    protected Rectangle GetDestRect(Rectangle? srcRect)
    {
        // take into account the scale and origin into 
        // the final result of dest rectangle
        
        if (srcRect == null) return new Rectangle();
        
        int width = (int)(srcRect.Value.Width * tm.scale.X);
        int height = (int)(srcRect.Value.Height * tm.scale.Y);
        Vector2 origin = GetOrigin();
        int pos_x = (int)(tm.position.X - origin.X * tm.scale.X);
        int pos_y = (int)(tm.position.Y - origin.Y * tm.scale.Y);
        
        return new Rectangle(
            pos_x,
            pos_y,
            width,
            height
        );
    }
    
}