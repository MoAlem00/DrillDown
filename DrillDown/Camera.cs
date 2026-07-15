using Microsoft.Xna.Framework;

namespace DrillDown;

public class Camera
{
    public Matrix position;

    public Camera()
    {
    }

    public void Follow(Player target)
    {
        position = Matrix.CreateTranslation(
            -target.tm.position.X + Game1._screenCenter.X,
            -target.tm.position.Y + Game1._screenCenter.Y,
            0);
    }
    
}