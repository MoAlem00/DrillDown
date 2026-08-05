using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class Sun : IUpdatable,IDrawable
{
    private Sprite sun;
    private float maxHeight = 1000f;
    private float t = 0;
    private float duration = 200f;
    

    public Sun(string spriteName)
    {
        sun = new Sprite(spriteName);
        sun.sortingOrder = 0f;
        sun.tm.position = new Vector2(Game1.groundLevel.X,Game1.groundLevel.Y);
    }
    
    private Vector2 MoveSun()
    {
        Vector2 start = new Vector2(Game1.groundLevel.X, Game1.groundLevel.Y);
        Vector2 end = new Vector2(Game1.groundLevel.X + Game1.columns * Game1.blockSize, Game1.groundLevel.Y);
        Vector2 flat = Vector2.Lerp(start, end, t);
        float arcHeight = maxHeight * 4 * t * (1 - t);
        return flat - new Vector2(0, arcHeight);
    }

    public void Start()
    {
        
    }

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        t += dt / duration;
        if (t > 1f) t = 0f;
        sun.tm.position = MoveSun();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        sun.Draw(spriteBatch);
    }
}