using Microsoft.Xna.Framework;

namespace DrillDown;

public class Camera
{
    public Vector2 position;

    public Camera(Vector2 position)
    {
        this.position = position;
    }

    public void Follow(Player target, Vector2 screenSize)
    {
        position = new Vector2(
            -target.tm.position.X + (screenSize.X / 2 - target.texture.Width / 2f), 
            -target.tm.position.Y + (screenSize.Y / 2 - target.texture.Height / 2f));
    }
    
}