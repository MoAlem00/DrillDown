using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class Menu : IDrawable,IUpdatable
{
    protected Panel panel;
    
    public void Start() { }
    
    public void Update(GameTime gameTime)
    {
        panel.UpdatePanel(gameTime);
    }
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        panel.DrawPanel(spriteBatch);
    }
}