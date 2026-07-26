using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace DrillDown;

public enum DrillDirection { None, Down, Left, Right}
public class Player : Animation
{
    private const float gravity = 200f;
    private Vector2 velocity;
    float speedMovement = 230;
    private float minFallSpeed = 300f;
    private float fallDamageMultiplier = 0.2f;
    bool isColliding = false;
    private bool isFlying = false;
    private bool isDead;
    private bool isGrounded;
    private float deltaTime;
    private bool showInv;
    private float startingCapacity = 100f;
    private float fuel;
    public float Fuel => fuel;
    private float maxFuel = 100f;
    public float MaxFuel => maxFuel;
    private float health;
    public float Health => health;
    private float maxHealth = 100f;
    public float MaxHealth => maxHealth;
    private float burnRate = 1f;
    private int money;
    public int Money => money;
    private Material ore;
    private Animation flame;
    private Animation explode;
    public World world { get; set; }
    public Collider collider { get; }
    private Inventory inventory;
    public Inventory Inventory => inventory;
    public event Action<float> OnFuelChange;
    public event Action<float> OnHealthChange;
    public event Action<int> OnMoneyChange;
    public event Action OnPlayerDeath;
    
    Vector2 prevPosition;
    
    private DrillDirection drillDirection = DrillDirection.None;
    private DrillDirection movingDirection = DrillDirection.None;
    private Dictionary<DrillDirection, Animation> animations = new();
    private Dictionary<DrillDirection, Vector2> offsets = new()
    {
        { DrillDirection.Down,  new Vector2(0, 32f) },
        { DrillDirection.Left,  new Vector2(-32f, 0) },
        { DrillDirection.Right, new Vector2(32f, 0) },
    };
    
    public Player() : base("DrillPod")
    {
        inventory = new Inventory(startingCapacity);
        flame = new Animation("Flame");
        explode = new Animation("Explosion");
        animations.Add(DrillDirection.Down, new Animation("DownDrill"));
        animations.Add(DrillDirection.Right, new Animation("RightDrill"));
        animations.Add(DrillDirection.Left, new Animation("LeftDrill"));
    }

    public override void Start()
    {
        base.Start();
        fuel = maxFuel;
        health = maxHealth;
        OnHealthChange?.Invoke(health / maxHealth);
        OnFuelChange?.Invoke(fuel / maxFuel);
        tm.position = new Vector2(Game1._screenCenter.X, Game1.groundLevel.Y - 30);
        tm.scale = new Vector2(0.8f, 0.8f);
        explode.tm.scale = new Vector2(2.5f, 2.5f);
        explode.sortingOrder = 1f;
        flame.tm.scale = new Vector2(1.4f, 1.4f);
        flame.PlayAnimation(true,5);
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
        if ((fuel <= 0 || health <=0) && !isDead)
            Die();

        if (isDead)
        {
            explode.tm.position = tm.position;
            explode.Update(gameTime);
            return;
        }
            
        if (movingDirection != DrillDirection.None || isFlying)
            BurnFuel(burnRate * deltaTime);
        
        MoveX();
        MoveY();
        
        if (!isGrounded)
            drillDirection = DrillDirection.None;
        
        int row = world.WorldToRow(tm.position);
        int col = world.WorldToCol(tm.position);
        
        switch (drillDirection)
        {
            case DrillDirection.Down: 
                ore = world.Drill(row + 1, col, deltaTime);
                AddOreToInventory(ore);
                break;
            case DrillDirection.Left: 
                ore = world.Drill(row, col - 1, deltaTime); 
                AddOreToInventory(ore);
                break;
            case DrillDirection.Right: 
                ore = world.Drill(row, col + 1, deltaTime);
                AddOreToInventory(ore);
                break;
        }
        
        if (drillDirection != DrillDirection.None)
        {
            animations[drillDirection].tm.position = tm.position + offsets[drillDirection];
            animations[drillDirection].Update(gameTime);
        }

        if (isFlying)
        {
            flame.tm.position = tm.position + offsets[DrillDirection.Down];
            flame.Update(gameTime);
        }
        base.Update(gameTime);
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        if(!isDead)
            base.Draw(spriteBatch);
        if (drillDirection != DrillDirection.None)
            animations[drillDirection].Draw(spriteBatch);
        if(isFlying)
            flame.Draw(spriteBatch);
        if(isDead)
            explode.Draw(spriteBatch);
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
            if (velocity.Y > 0) isGrounded = true;
            if (movingDirection == DrillDirection.Down)
                drillDirection = DrillDirection.Down;
            tm.position.Y = prevPosition.Y;
            if (velocity.Y > minFallSpeed)
            {
                float fallDamage = (velocity.Y - minFallSpeed) * fallDamageMultiplier;
                LoseHealth(fallDamage);
            }
            velocity.Y = 0;
        }
    }
    

    private void AddOreToInventory(Material material)
    {
        if (material != null)
        {
            inventory.TryAddMaterial(material);
            Console.WriteLine($"Collected {material.Name}");
        }
    }

    private void ShowInventory()
    {
        foreach (var item in inventory)
        {
            Console.WriteLine($"{item}");
        }
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
            isFlying = true;
            velocity.Y = -speedMovement;
        }
        else isFlying = false;
        
    }

    private void Die()
    {
        isDead = true;
        explode.PlayAnimation(false,13);
        OnPlayerDeath?.Invoke();
    }

    private void BurnFuel(float amount)
    {
        fuel = Math.Clamp(fuel - amount, 0f, maxFuel);
        OnFuelChange?.Invoke(fuel / maxFuel);
    }

    private void LoseHealth(float amount)
    {
        health = Math.Clamp(health - amount, 0f, maxHealth);
        OnHealthChange?.Invoke(health / maxHealth);
    }

    public void Refuel(float amount)
    {
        fuel = Math.Clamp(fuel + amount, 0f, maxFuel);
        OnFuelChange?.Invoke(fuel / maxFuel);
    }

    public void Repair(float amount)
    {
        health = Math.Clamp(health + amount, 0f, maxHealth);
        OnHealthChange?.Invoke(health / maxHealth);
    }
    public void AddMoney(int amount)
    {
        money += amount;
        OnMoneyChange?.Invoke(money);
    }

    public bool TrySpendMoney(int amount)
    {
        if (money < amount)
            return false;
        money -= amount;
        OnMoneyChange?.Invoke(money);
        return true;
    }
}