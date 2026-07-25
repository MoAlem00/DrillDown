using Microsoft.Xna.Framework;

namespace DrillDown;

public class Camera
{
    public Matrix position;
    private World world;

    public Camera(World world)
    {
        this.world = world;
    }

    public void Follow(Player target)
    {
        float halfW = Game1._screenCenter.X;
        float halfH = Game1._screenCenter.Y;
        float x = MathHelper.Clamp(target.tm.position.X,world.GetWorldLeft()+halfW,world.GetWorldRight()-halfW);
        float y = MathHelper.Min(target.tm.position.Y,world.GetWorldBottom() - halfH);
        position = Matrix.CreateTranslation(
            -x + halfW,-y + halfH,0);
    }
    
}