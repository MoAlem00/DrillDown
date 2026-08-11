using System;
using Microsoft.Xna.Framework;

namespace DrillDown;

public class Camera
{
    public Matrix position;
    private World world;
    private float shakeTimer;
    private float shakeIntensity;
    private Random random = new Random();

    public Camera(World world)
    {
        this.world = world;
    }

    public void Follow(Player target,GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        float halfW = Game1._screenCenter.X;
        float halfH = Game1._screenCenter.Y;
        float x = MathHelper.Clamp(target.tm.position.X,world.GetWorldLeft()+halfW,world.GetWorldRight()-halfW);
        float y = MathHelper.Min(target.tm.position.Y,world.GetWorldBottom() - halfH);
        float shakeX = 0f, shakeY = 0f;
        if (shakeTimer > 0f)
        {
            shakeTimer -= deltaTime;
            shakeX = (float)(random.NextDouble() * 2 - 1) * shakeIntensity;
            shakeY = (float)(random.NextDouble() * 2 - 1) * shakeIntensity;
        }

        position = Matrix.CreateTranslation(
            -x + halfW + shakeX,-y + halfH + shakeY,0);
    }

    public void CameraShake(float duration, float intensity)
    {
        shakeTimer = duration;
        shakeIntensity = intensity;
    }
    
}