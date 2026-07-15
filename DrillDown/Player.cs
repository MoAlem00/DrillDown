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
    public Animation bottomDrill;
    public Animation rightDrill;
    public Animation leftDrill;
    private bool isDrillingBottom;
    private bool isDrillingRight;
    private bool isDrillingLeft;
    public World world { get; set; }
    public Collider collider { get; }
    Vector2 prevPosition = Vector2.Zero;
    public Player() : base("DrillPod")
    {
        collider = SceneManager.Create<Collider>();
        collider.Parent = this;
        bottomDrill = new Animation("DownDrill");
        rightDrill = new Animation("RightDrill");
        leftDrill = new Animation("LeftDrill");
    }

    public override void Start()
    {
        base.Start();
        tm.position = Game1._screenCenter;
        tm.scale = new Vector2(0.8f, 0.8f);
        prevPosition =  tm.position;
        bottomDrill.PlayAnimation();
        rightDrill.PlayAnimation();
        leftDrill.PlayAnimation();
    }

    public override void Update(GameTime gameTime)
    {
        isDrillingBottom = false;
        isDrillingRight = false;
        isDrillingLeft = false;
        bottomDrill.tm.position = tm.position + new Vector2(0f, 32f);
        leftDrill.tm.position = tm.position + new Vector2(-32f,0f);
        rightDrill.tm.position = tm.position + new Vector2(32f,0f);
        int row = world.WorldToRow(tm.position);
        int col = world.WorldToCol(tm.position);
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        velocity.X = 0;
        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            world.Drill(row, col + 1, deltaTime);
            isDrillingRight = true;
            effects = SpriteEffects.FlipHorizontally;
            velocity.X = speedMovement;
            
        }
        
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            world.Drill(row, col - 1, deltaTime);
            isDrillingLeft = true;
            effects = SpriteEffects.None;
            velocity.X = -speedMovement;
        }
        
        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            world.Drill(row + 1, col, deltaTime);
            isDrillingBottom = true;
            velocity.Y = speedMovement;
        }
        
        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            velocity.Y = -speedMovement;
        }
        velocity.Y += gravity * deltaTime;
        tm.position += velocity * deltaTime;
        
        bottomDrill.Update(gameTime);
        rightDrill.Update(gameTime);
        leftDrill.Update(gameTime);
        base.Update(gameTime);
        
        destRect = GetDestRect(sourceRect);
        if (world.IsSolid(destRect))
        {
            tm.position = prevPosition;
            velocity.Y = 0f;
        }
        prevPosition =  tm.position;
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        if (isDrillingBottom)
            bottomDrill.Draw(spriteBatch);
        if (isDrillingRight)
            rightDrill.Draw(spriteBatch);
        if (isDrillingLeft)
            leftDrill.Draw(spriteBatch);
        
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