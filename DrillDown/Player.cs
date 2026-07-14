using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace DrillDown;

public class Player : Animation
{
    bool IsRKeyPressed = false;
    float speedRotation = 0;
    float speedMovement = 100;
    bool isColliding = false;
    public World world { get; set; }
    public Collider collider { get; }
    Vector2 prevPosition = Vector2.Zero;
    public Player() : base("DrillPod")
    {
        collider = SceneManager.Create<Collider>();
        collider.Parent = this;
    }

    public override void Start()
    {
        base.Start();

        tm.position = Game1._screenCenter;
        tm.scale = new Vector2(1f, 1f);
        prevPosition =  tm.position;
    }

    public override void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (Keyboard.GetState().IsKeyDown(Keys.R) && !IsRKeyPressed)
        {
            // R was pressed in this frame
            speedRotation = 500;
        }
        
        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            effects = SpriteEffects.FlipHorizontally;
            tm.position += new Vector2(speedMovement * deltaTime, 0);
        }
        
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            effects = SpriteEffects.None;
            tm.position += new Vector2(-speedMovement * deltaTime, 0);
        }
        
        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            tm.position += new Vector2(0, speedMovement * deltaTime);
        }
        
        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            tm.position += new Vector2(0, -speedMovement * deltaTime);
        }

        IsRKeyPressed =  Keyboard.GetState().IsKeyDown(Keys.R);
        
        tm.rotation = (float)gameTime.TotalGameTime.TotalSeconds * speedRotation;

        base.Update(gameTime);
        
        /*if (isColliding)
        {
            tm.position =  prevPosition;
            isColliding = false;
        }*/
        destRect = GetDestRect(sourceRect);
        if (world.IsSolid(destRect))
            tm.position = prevPosition;
        prevPosition =  tm.position;
    }
    public void OnCollision(Collider selfCollider, Collider otherCollider)
    {
        isColliding = true;
        Console.WriteLine("Self " + selfCollider.Parent + " is colliding with " + otherCollider.Parent);
    }
    
    public void OnTrigger(Collider selfCollider, Collider otherCollider)
    {
        Console.WriteLine("Self " + selfCollider.Parent + " is trigger with " + otherCollider.Parent);
        
        SceneManager.Remove(otherCollider);
        SceneManager.Remove(otherCollider.Parent);
    }
}