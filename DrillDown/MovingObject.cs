using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class MovingObject : IUpdatable,IDrawable
{
    private Sprite sprite;
    private float maxHeight;
    private float t = 0;
    private float duration;
    private Vector2 startPoint;
    private Vector2 endPoint;
    

    public MovingObject(string spriteName,float maxHeight,float duration,Vector2 startPoint,Vector2 endPoint)
    {
        sprite = new Sprite(spriteName);
        sprite.sortingOrder = 0f;
        this.maxHeight = maxHeight;
        this.duration = duration;
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        sprite.tm.position = startPoint;
    }
    
    private Vector2 MoveObject()
    {
        Vector2 start = startPoint;
        Vector2 end = endPoint;
        Vector2 flat = Vector2.Lerp(start, end, t);
        float arcHeight = maxHeight * 4 * t * (1 - t);
        return flat - new Vector2(0, arcHeight);
    }

    public void Start()
    {
        
    }

    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        t += deltaTime / duration;
        if (t > 1f) t = 0f;
        sprite.tm.position = MoveObject();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        sprite.Draw(spriteBatch);
    }
}