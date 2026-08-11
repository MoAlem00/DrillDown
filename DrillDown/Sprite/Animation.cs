using System;
using Microsoft.Xna.Framework;
namespace DrillDown;

public class Animation : Sprite
{
    private double totalTime = 0;
    private int samples = 60;
    private int x = 0;
    private int y = 0;
    bool isLooping = true;
    bool isAnimating = false;
    private bool isFinished = false;
    public bool IsFinished => isFinished;
    public Animation(string spriteName) : base(spriteName)
    {
    }
    
    public void PlayAnimation(bool isLooping = true, int samples = 60)
    {
        this.isLooping = isLooping;
        this.samples = samples;
        Reset();
        isAnimating = true;
    }

    public void StopAnimation()
    {
        Reset();
    }

    void PauseAnimation()
    {
        isAnimating = false;
    }
    void ResumeAnimation()
    {
        isAnimating = true;
    }

    void Reset()
    {
        isAnimating = false;
        x = y = 0;
        totalTime = 0;
    }

    public override void Update(GameTime gameTime)
    { 
        if (!isAnimating) return;
        
        if (CanMoveFrame(gameTime))
            MoveFrame();
       
        base.Update(gameTime);
    }

    bool CanMoveFrame(GameTime gameTime)
    {
        double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;
        totalTime += deltaTime;
        
        if (totalTime >= 1.0f / samples)
            return true;

        return false;
    }

    void MoveFrame()
    {
        totalTime = 0;
        x++;

        if (x == spriteSheet.columns)
        {
            x = 0;
            y++;
            if (y == spriteSheet.rows)
            {
                if (isLooping)
                {
                    x = 0;
                    y = 0;
                }
                else
                {
                    x = spriteSheet.columns - 1;
                    y = spriteSheet.rows - 1;
                    isFinished = true;
                    isAnimating = false;
                }
            }
        }

        sourceRect = spriteSheet[x, y];
        
        destRect = GetDestRect(sourceRect);
    }
}