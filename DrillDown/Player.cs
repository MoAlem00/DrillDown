using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace DrillDown;

public enum DrillDirection { None, Down, Left, Right}
public class Player : Animation
{
    private const float gravity = 500f;
    private Vector2 velocity;
    private float speedMovement = 250f; //230
    private float drillSpeed = 150f;
    private float flySpeed = 350f;
    private float minFallSpeed = 500f; //300
    private float fallDamageMultiplier = 0.2f;
    private bool isFlying;
    private bool isDead;
    private bool isGrounded;
    private float deltaTime;
    private float startingCapacity = 50f;
    private float fuel;
    private float maxFuel = 50f;
    private float health;
    private float maxHealth = 100f;
    private float burnRate = 0.5f;
    private int money;
    private float drillPower = 1f;
    private float maxDrillPower = 4f;
    private bool deathFired;
    private Vector2 prevPosition;
    private Material ore;
    private Animation flame;
    private Animation explode;
    public World world;
    private Inventory inventory;
    public Inventory Inventory => inventory;
    public event Action<float> OnFuelChange;
    public event Action<float> OnHealthChange;
    public event Action<int> OnMoneyChange;
    public event Action OnHardLanding;
    public event Action OnPlayerDeath;
    public float Fuel => fuel;
    public float MaxFuel => maxFuel;
    public float Health => health;
    public float MaxHealth => maxHealth;
    public int Money => money;
    public float DrillPower => drillPower;
    private SoundEffectInstance drillSound;
    private SoundEffectInstance fuelWarningSfx;
    private SoundEffectInstance thrustSound;
    private bool wasKeyPressed;
    private bool wasTKeyPressed;
    private bool wasRKeyPressed;
    private bool wasFKeyPressed;
    private bool wasBKeyPressed;
    public EffectManager effectManager;
    
    
    private DrillDirection drillDirection = DrillDirection.None;
    private DrillDirection movingDirection = DrillDirection.None;
    private Dictionary<DrillDirection, Animation> animations = new();
    private Dictionary<DrillDirection, Vector2> offsets = new()
    {
        { DrillDirection.Down,  new Vector2(0, 32f) },
        { DrillDirection.Left,  new Vector2(-32f, 0) },
        { DrillDirection.Right, new Vector2(32f, 0) },
    };
    
    public Player(World world,EffectManager effectManager) : base("DrillPod")
    {
        this.world = world;
        this.effectManager = effectManager;
        anchor = Anchor.Center;
        inventory = new Inventory(startingCapacity);
        flame = new Animation("Flame");
        flame.anchor = Anchor.Center;
        explode = new Animation("Explosion");
        explode.anchor = Anchor.Center;
        animations.Add(DrillDirection.Down, new Animation("DownDrill") { anchor = Anchor.Center });
        animations.Add(DrillDirection.Right, new Animation("RightDrill") { anchor = Anchor.Center });
        animations.Add(DrillDirection.Left, new Animation("LeftDrill") { anchor = Anchor.Center });
        drillSound = AudioManager.CreateSfxInstance("drillSfx",0.2f);
        fuelWarningSfx = AudioManager.CreateSfxInstance("fuelWarningSfx",0.5f);
        thrustSound = AudioManager.CreateSfxInstance("Thrust",0.3f);
        drillSound.Pitch = -0.5f;
    }

    public override void Start()
    {
        base.Start();
        fuel = maxFuel;
        health = maxHealth;
        OnHealthChange?.Invoke(health / maxHealth);
        OnFuelChange?.Invoke(fuel / maxFuel);
        tm.position = new Vector2(Game1._screenCenter.X, Game1.groundLevel.Y - 55);
        tm.scale = new Vector2(0.8f, 0.8f);
        explode.tm.scale = new Vector2(2.5f, 2.5f);
        flame.tm.scale = new Vector2(1.4f, 1.4f);
        flame.PlayAnimation(true,5);
        foreach (Animation animation in animations.Values)
        {
            animation.PlayAnimation();
        }
        sortingOrder = 5f/Game1.totalLayers;
        flame.sortingOrder = 4.8f/Game1.totalLayers;
        explode.sortingOrder = 10f/Game1.totalLayers;
        foreach (var a in animations)
        {
            a.Value.sortingOrder = 5f/Game1.totalLayers;
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
            if (explode.IsFinished && !deathFired)
            {
                deathFired = true;
                OnPlayerDeath?.Invoke();
            }
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
        
        float drillAmount = deltaTime * drillPower;
        switch (drillDirection)
        {
            case DrillDirection.Down:
                ore = world.Drill(row + 1, col, drillAmount);
                AddOreToInventory(ore);
                break;
            case DrillDirection.Left: 
                ore = world.Drill(row, col - 1, drillAmount); 
                AddOreToInventory(ore);
                break;
            case DrillDirection.Right: 
                ore = world.Drill(row, col + 1, drillAmount);
                AddOreToInventory(ore);
                break;
        }
        bool isDrilling = drillDirection != DrillDirection.None && isGrounded;
        if (isDrilling && drillSound.State != SoundState.Playing)
            drillSound.Play();
        else if (!isDrilling && drillSound.State == SoundState.Playing)
            drillSound.Stop();
        
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
    //New Position += v × dt
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
                OnHardLanding?.Invoke();
                effectManager.SpawnEffect("LandEffect", tm.position);
                AudioManager.PlaySoundEffect("Impact",false,0.2f);
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
    

    private void UseItem(ItemType type)
    {
        Item item = inventory.GetItem(type);
        if (item != null)
            item.CanUseItem(this);
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
        KeyboardState keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.S))
        {
            movingDirection = DrillDirection.Down;
            if(isGrounded)
                velocity.Y = drillSpeed;
        }
        else if (keyboardState.IsKeyDown(Keys.D))
        {
            movingDirection = DrillDirection.Right;
            effects = SpriteEffects.FlipHorizontally;
            velocity.X = speedMovement;
        }
        else if (keyboardState.IsKeyDown(Keys.A))
        {
            movingDirection = DrillDirection.Left;
            effects = SpriteEffects.None;
            velocity.X = -speedMovement;
        }
        if (keyboardState.IsKeyDown(Keys.W))
        {
            if (thrustSound.State != SoundState.Playing)
                thrustSound.Play();
            isFlying = true;
            velocity.Y = -flySpeed;
        }
        else
        {
            if (thrustSound.State == SoundState.Playing)
                thrustSound.Stop();
            isFlying = false;
        }
        
        ItemsInput(keyboardState);
        CheatsInput(keyboardState);
    }

    

    private void Die()
    {
        isDead = true;
        explode.PlayAnimation(false,13);
        AudioManager.PlaySoundEffect("explosion");
    }

    private void BurnFuel(float amount)
    {
        fuel = Math.Clamp(fuel - amount, 0f, maxFuel);
        if(fuel <= maxFuel/3f) fuelWarningSfx.Play();
        else fuelWarningSfx.Stop();
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
        fuelWarningSfx.Stop();
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

    public void UpgradeDrill(float amount)
    {
        drillPower = Math.Clamp(drillPower + amount, 0, maxDrillPower);
    }

    public void UpgradeFuel(float amount)
    {
        maxFuel = Math.Clamp(maxFuel + amount, 0, 500f);
        OnFuelChange?.Invoke(fuel / maxFuel);
    }

    public void UpgradeArmor(float amount)
    {
        maxHealth = Math.Clamp(maxHealth + amount, 0, 500f);
        OnHealthChange?.Invoke(health / maxHealth);
    }

    public void Teleport()
    {
        tm.position = Game1._screenCenter;
    }

    private void CheatMoney()
    {
        money += 1000000;
        OnMoneyChange?.Invoke(money);
    }

    private void Cheat()
    {
        drillPower = 100;
    }

    private void ShowStats()
    {
        Console.WriteLine($"Drill Power: {drillPower}\r\nMax Health: {maxHealth}\r\nMax Fuel: {maxFuel}\r\nCapacity: {inventory.Capacity}");
    }
    
    private void CheatsInput(KeyboardState keyboardState)
    {
        if (keyboardState.IsKeyDown(Keys.T)&&keyboardState.IsKeyDown(Keys.P))
            Teleport();
        if (keyboardState.IsKeyDown(Keys.M)&&keyboardState.IsKeyDown(Keys.A)&&keyboardState.IsKeyDown(Keys.X))
            Cheat();
        if(keyboardState.IsKeyDown(Keys.RightShift)&&keyboardState.IsKeyDown(Keys.D4))
            CheatMoney();
        if (keyboardState.IsKeyDown(Keys.L))
            ShowStats();
    }
    private void ItemsInput(KeyboardState keyboardState)
    {
        bool tPressed = keyboardState.IsKeyDown(Keys.T);
        if (tPressed && !wasTKeyPressed)
            UseItem(ItemType.Teleport);
        wasTKeyPressed = tPressed;
        bool rPressed = keyboardState.IsKeyDown(Keys.R);
        if(rPressed && !wasRKeyPressed)
            UseItem(ItemType.RepairKit);
        wasRKeyPressed = rPressed;
        bool fPressed = keyboardState.IsKeyDown(Keys.F);
        if (fPressed && !wasFKeyPressed)
            UseItem(ItemType.FuelTank);
        wasFKeyPressed = fPressed;
        bool bPressed = keyboardState.IsKeyDown(Keys.B);
        if (bPressed && !wasBKeyPressed)
            UseItem(ItemType.Bomb);
        wasBKeyPressed = bPressed;
    }
}