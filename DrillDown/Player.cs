using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace DrillDown;

public class Player : Animation
{
    private const float gravity = 150f;
    private Vector2 velocity;
    float speedMovement = 100;
    bool isColliding = false;
    private float fuel {get; set;}
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
        tm.scale = new Vector2(0.8f, 0.8f);
        prevPosition =  tm.position;
    }

    public override void Update(GameTime gameTime)
    {
        int row = world.WorldToRow(tm.position);
        int col = world.WorldToCol(tm.position);
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        velocity.X = 0;
        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            effects = SpriteEffects.FlipHorizontally;
            velocity.X = speedMovement;
            world.Drill(row, col + 1, deltaTime);
        }
        
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            world.Drill(row, col - 1, deltaTime);
            effects = SpriteEffects.None;
            velocity.X = -speedMovement;
        }
        
        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            world.Drill(row + 1, col, deltaTime);
            velocity.Y = speedMovement;
        }
        
        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            velocity.Y = -speedMovement;
        }
        velocity.Y += gravity * deltaTime;
        tm.position += velocity * deltaTime;

        base.Update(gameTime);
        
        destRect = GetDestRect(sourceRect);
        if (world.IsSolid(destRect))
        {
            tm.position = prevPosition;
            velocity.Y = 0f;
        }
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