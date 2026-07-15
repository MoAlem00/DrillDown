using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace DrillDown;

public enum DrillDirection { None, Down, Left, Right }
public class Player : Animation
{
    private const float gravity = 150f;
    private Vector2 velocity;
    float speedMovement = 100;
    bool isColliding = false;
    private bool isGrounded;
    private float deltaTime;
    private float fuel {get; set;}
    public World world { get; set; }
    public Collider collider { get; }
    Vector2 prevPosition;
    private DrillDirection drillDirection = DrillDirection.None;
    private DrillDirection movingDirection = DrillDirection.None;
    private Dictionary<DrillDirection, Animation> animations = new Dictionary<DrillDirection, Animation>();
    private Dictionary<DrillDirection, Vector2> offsets = new()
    {
        { DrillDirection.Down,  new Vector2(0, 32f) },
        { DrillDirection.Left,  new Vector2(-32f, 0) },
        { DrillDirection.Right, new Vector2(32f, 0) },
    };
    
    public Player() : base("DrillPod")
    {
        collider = SceneManager.Create<Collider>();
        collider.Parent = this;
        animations.Add(DrillDirection.Down, new Animation("DownDrill"));
        animations.Add(DrillDirection.Right, new Animation("RightDrill"));
        animations.Add(DrillDirection.Left, new Animation("LeftDrill"));
    }

    public override void Start()
    {
        base.Start();
        tm.position = Game1._screenCenter;
        tm.scale = new Vector2(0.8f, 0.8f);
        foreach (Animation animation in animations.Values)
        {
            animation.PlayAnimation();
        }

    }

    public override void Update(GameTime gameTime)
    {
        deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        prevPosition = tm.position;
        
        ReadInput();
        
        MoveX();
        MoveY();
        
        if (!isGrounded)
            drillDirection = DrillDirection.None;
        
        int row = world.WorldToRow(tm.position);
        int col = world.WorldToCol(tm.position);
        
        switch (drillDirection)
        {
            case DrillDirection.Down: world.Drill(row + 1, col, deltaTime); break;
            case DrillDirection.Left: world.Drill(row, col - 1, deltaTime); break;
            case DrillDirection.Right: world.Drill(row, col + 1, deltaTime); break;
        }

        if (drillDirection != DrillDirection.None)
        {
            animations[drillDirection].tm.position = tm.position + offsets[drillDirection];
            animations[drillDirection].Update(gameTime);
        }
        base.Update(gameTime);
        
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        if (drillDirection != DrillDirection.None)
            animations[drillDirection].Draw(spriteBatch);
        
    }

    private void MoveX()
    {
        tm.position.X += velocity.X * deltaTime;
        destRect = GetDestRect(sourceRect);
        if (world.IsSolid(destRect))
        {
            tm.position.X = prevPosition.X;
            if(movingDirection == DrillDirection.Left)
                drillDirection = DrillDirection.Left;
            else if(movingDirection == DrillDirection.Right)
                drillDirection = DrillDirection.Right;
        }
    }

    private void MoveY()
    {
        velocity.Y += gravity * deltaTime;
        tm.position.Y += velocity.Y * deltaTime;
        destRect = GetDestRect(sourceRect);
        isGrounded = false;
        if (world.IsSolid(destRect))
        {
            if(velocity.Y > 0) isGrounded = true;
            if (movingDirection == DrillDirection.Down)
                drillDirection = DrillDirection.Down;
            tm.position.Y = prevPosition.Y;
            velocity.Y = 0;
        }
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
    
    private void ReadInput()
    {
        velocity.X = 0;
        drillDirection = DrillDirection.None;
        movingDirection = DrillDirection.None;
        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            movingDirection = DrillDirection.Down;
            velocity.Y = speedMovement;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            movingDirection = DrillDirection.Right;
            effects = SpriteEffects.FlipHorizontally;
            velocity.X = speedMovement;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            movingDirection = DrillDirection.Left;
            effects = SpriteEffects.None;
            velocity.X = -speedMovement;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            velocity.Y = -speedMovement;
        }
    }
}