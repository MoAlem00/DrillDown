using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DrillDown;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    public static int totalLayers = 10;
    public static Vector2 _screenCenter;
    public static Vector2 _screenLeftCenter;
    public static Vector2 _screenRightCorner;
    public static Vector2 _screenLeftCorner;
    public static Vector2 _screenTopCenter;
    public static int _screenWidth;
    public const int blockSize = 64;
    public const int columns = 55;
    public const int rows = 500;
    private Player player;
    public static SpriteFont _font;
    public static Vector2 groundLevel;
    private Vector2 yLevelOffset = new Vector2(0, 200f);
    private SpriteManager spriteManager;
    private WorldGenerator worldGenerator;
    private Block[,] grid;
    private World world;
    private Camera camera;
    private HUD hud;
    private StartMenu startMenu;
    private FinishMenu finishMenu;
    private GameOverMenu gameOverMenu;
    private List<Shop> shops = new();
    private List<Zone> zones = new();
    private List<Menu> menus = new();
    private Portal portal;
    private Sprite underGround;
    private List<MovingObject> movingObjects = new();
    private Vector2 startPoint;
    private Vector2 endPoint;
    private int tilesX, tilesY, tileW, tileH;
    private EffectManager effectManager;

    #region ResourcesManager
    
    private ResourcesManager<Song> songManager;
    private ResourcesManager<SoundEffect> soundEffectManager;

    #endregion
    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        spriteManager = new SpriteManager(Content);
        songManager = new ResourcesManager<Song>(Content);
        soundEffectManager = new ResourcesManager<SoundEffect>(Content);
        
        
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.PreferredBackBufferHeight = 1080;

        _graphics.IsFullScreen = false;
        
        _screenCenter =  new Vector2(
            _graphics.PreferredBackBufferWidth * 0.5f,
            _graphics.PreferredBackBufferHeight * 0.5f);
        
        _screenLeftCenter = new Vector2(0, 
            _graphics.PreferredBackBufferHeight * 0.5f);
        _screenWidth = _graphics.PreferredBackBufferWidth;
        _screenRightCorner = new Vector2(_graphics.PreferredBackBufferWidth, 0);

        _screenTopCenter = new Vector2(_graphics.PreferredBackBufferWidth * 0.5f, 0);
        
        _screenLeftCorner = Vector2.Zero;
        groundLevel = _screenLeftCenter + yLevelOffset;
        startPoint = new Vector2(groundLevel.X,groundLevel.Y);
        endPoint = new Vector2(groundLevel.X + columns * blockSize,groundLevel.Y);
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("Fonts/GeistPixel");
        Button.Pixel = new Texture2D(GraphicsDevice, 1, 1);
        Button.Pixel.SetData(new Color[] { Color.White });
        
        AddAudio();
        AddGameSprites();
        AddMovingObjects();
        CreateBlocksOresZones();

        Start();
    }
    private void Start()
    {   
        StartGame();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        camera.Follow(player,gameTime);
        switch (GameManager.Instance.gameState)
        {
            case GameManager.GameState.MainMenu:
                startMenu.Update(gameTime);
                break;
            case GameManager.GameState.Playing:
                effectManager.Update(gameTime);
                underGround.Update(gameTime);
                player.Update(gameTime);
                portal.Update(gameTime);
                foreach (var obj in movingObjects)
                {
                    obj.Update(gameTime);
                }
                foreach (var shop in shops)
                {
                    shop.Update();
                    shop.UpdatePanel(gameTime);
                }
                hud.Update(gameTime);
                break;
            case GameManager.GameState.GameOver:
                gameOverMenu.Update(gameTime);
                break;
            case GameManager.GameState.WinGame:
                finishMenu.Update(gameTime);
                break;
        }
        AudioManager.Update();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.SkyBlue);

        _spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp,
            transformMatrix: camera.position);

        switch (GameManager.Instance.gameState)
        {
            case GameManager.GameState.Playing:
                DrawBackground(_spriteBatch);
                effectManager.Draw(_spriteBatch);
                world.Draw(_spriteBatch);
                foreach (var movingObject in movingObjects)
                    movingObject.Draw(_spriteBatch);
                portal.Draw(_spriteBatch);
                portal.DrawPrompt(_spriteBatch);
                player.Draw(_spriteBatch);
                foreach (var shop in shops)
                {
                    shop.Draw(_spriteBatch);
                    shop.DrawPrompt(_spriteBatch);
                }
                break;
        }
        _spriteBatch.End();
        
        _spriteBatch.Begin();
        hud.Draw(_spriteBatch);
        switch (GameManager.Instance.gameState)
        {
            case GameManager.GameState.Playing:
                foreach (var shop in shops)
                    shop.DrawPanel(_spriteBatch);
                break;
            case GameManager.GameState.MainMenu:
                startMenu.Draw(_spriteBatch);
                break;
            case GameManager.GameState.GameOver:
                gameOverMenu.Draw(_spriteBatch);
                break;
            case GameManager.GameState.WinGame:
                finishMenu.Draw(_spriteBatch);
                break;
        }
        _spriteBatch.End();
        base.Draw(gameTime);
    }

    private void DrawBackground(SpriteBatch spriteBatch)
    {
        for (int y = 0; y < tilesY; y++)
        {
            for (int x = 0; x < tilesX; x++)
            {
                underGround.tm.position = new Vector2(
                    groundLevel.X + x * tileW,
                    groundLevel.Y + y * tileH);
                underGround.Draw(_spriteBatch);
            }
        }
    }
    private void MakeBlocksBelowShopsUnbreakable(List<Shop> shops)
    {
        foreach (Shop shop in shops)
        {
            Vector2 shopPos = shop.GetShopPosition();
            int blocksCovered = shop.GetShopWidthInBlocks();
            int shopStartingCol = world.WorldToCol(shopPos);
            for (int c = shopStartingCol ; c < blocksCovered + shopStartingCol; c++)
                world.SetBlockUnbreakable(0,c);
        }
    }
    

    private void AddMovingObjects()
    {
        float cloudsPos = 500f;
        Vector2 c1, c2, c3, c4;
        c1 = startPoint - new Vector2(0, cloudsPos);
        c2 = c1 + new Vector2(400, -50);
        c3 = c1 + new Vector2(800, 30);
        c4 = c1 + new Vector2(1200, -20);
        movingObjects.Add(new MovingObject("Sun",1000f,200f,startPoint,endPoint));
        movingObjects.Add(new MovingObject("Cloud1",50f,300f,c1,new Vector2(endPoint.X,c1.Y)));
        movingObjects.Add(new MovingObject("Cloud2",70f,280f,c2,new Vector2(endPoint.X,c2.Y)));
        movingObjects.Add(new MovingObject("Cloud3",0f,260f,c3,new Vector2(endPoint.X,c3.Y)));
        movingObjects.Add(new MovingObject("Cloud2",100f,100f,c4,new Vector2(endPoint.X,c4.Y)));
    }

    private void TilesCount()
    {
        tileW = underGround.texture.Width;
        tileH = underGround.texture.Height;

        float worldWidth = columns * blockSize;
        float worldBottom = groundLevel.Y + rows * blockSize;
        
        tilesX = (int)Math.Ceiling(worldWidth / tileW);
        tilesY = (int)Math.Ceiling((worldBottom - groundLevel.Y) / tileH);
    }

    private void StartGame()
    {
        effectManager = new EffectManager();
        underGround = new Sprite("UnderGround");
        underGround.sortingOrder = 0.1f / totalLayers;
        worldGenerator = new WorldGenerator(rows, columns,zones);
        grid = worldGenerator.GenerateWorld();
        world = new World(grid, blockSize, groundLevel, 2f/totalLayers);
        camera = new Camera(world);
        player = new Player(world,effectManager);
        hud = new HUD(player.Inventory);
        player.PlayAnimation();
        player.OnFuelChange += hud.HandleFuelChange;
        player.OnHealthChange += hud.HandleHealthChange;
        player.OnMoneyChange += hud.HandleMoneyChange;
        player.Inventory.OnCapacityChange += hud.HandleCapacityChange;
        player.OnPlayerDeath += GameManager.Instance.HandleGameOver;
        player.OnHardLanding += () => camera.CameraShake(0.15f, 9f);
        player.Inventory.OnInventoryFull += hud.HandleInventoryFull;
        player.Inventory.OnInventoryEmpty += hud.HandleInventoryEmpty;
        world.OnBlockBreak += effectManager.SpawnEffect;
        GameManager.Instance.OnQuitGame += Exit;
        GameManager.Instance.OnGameRestart += ResetGame;
        foreach (var item in player.Inventory.Items.Values)
            item.OnUse += effectManager.SpawnEffect;
        
        player.Start();
        shops.Add(new GasStation("GasStation",0.4f, 2,player));
        shops.Add(new UpgradesShop("UpgradesShop",0.3f, 10,player));
        shops.Add(new MineralsShop("MineralsShop",0.5f,20,player));
        shops.Add(new ItemsShop("ItemsShop",1f, 35,player));
        shops.Add(new RepairStation("RepairStation", 0.3f, 47,player));
        portal = new Portal("Portal", 3f, player);
        portal.PlaceAtRandomPosition();
        MakeBlocksBelowShopsUnbreakable(shops);
        startMenu = new StartMenu(new Sprite("MenuBackground"));
        finishMenu = new FinishMenu();
        gameOverMenu = new GameOverMenu();
        TilesCount();
    }

    private void ResetGame()
    {
        player.OnFuelChange -= hud.HandleFuelChange;
        player.OnHealthChange -= hud.HandleHealthChange;
        player.OnMoneyChange -= hud.HandleMoneyChange;
        player.Inventory.OnCapacityChange -= hud.HandleCapacityChange;
        player.OnPlayerDeath -= GameManager.Instance.HandleGameOver;
        player.OnHardLanding -= () => camera.CameraShake(0.15f, 9f);
        world.OnBlockBreak -= effectManager.SpawnEffect;
        foreach (var item in player.Inventory.Items.Values)
            item.OnUse -= effectManager.SpawnEffect;
        GameManager.Instance.OnQuitGame -= Exit;
        shops.Clear();
        StartGame();
    }
    
    private void CreateBlocksOresZones()
    {
        Material coalOre = new Material("CoalOre",SpriteManager.GetSprite("CoalOre").texture,0.5f,20);
        Material ironOre = new Material("IronOre",SpriteManager.GetSprite("IronOre").texture,2f,30);
        Material copperOre = new Material("CopperOre",SpriteManager.GetSprite("CopperOre").texture,2f,45);
        Material silverOre = new Material("SilverOre",SpriteManager.GetSprite("SilverOre").texture,2.5f,90);
        Material goldOre = new Material("GoldOre",SpriteManager.GetSprite("GoldOre").texture,4f,180);
        Material titaniumOre = new Material("TitaniumOre",SpriteManager.GetSprite("TitaniumOre").texture,1.5f,250);
        Material amethystOre = new Material("AmethystOre",SpriteManager.GetSprite("AmethystOre").texture,1.5f,300);
        Material platinumOre = new Material("PlatinumOre",SpriteManager.GetSprite("PlatinumOre").texture,4.5f,450);
        Material sapphireOre = new Material("SapphireOre",SpriteManager.GetSprite("SapphireOre").texture,2f,600);
        Material rubyOre = new Material("RubyOre",SpriteManager.GetSprite("RubyOre").texture,2f,850);
        Material emeraldOre = new Material("EmeraldOre",SpriteManager.GetSprite("EmeraldOre").texture,2f,1200);
        Material opalOre = new Material("OpalOre",SpriteManager.GetSprite("OpalOre").texture,1.5f,2000);
        Material diamondOre = new Material("DiamondOre",SpriteManager.GetSprite("DiamondOre").texture,1f,5000);
        Material kryptoniteOre = new Material("KryptoniteOre",SpriteManager.GetSprite("KryptoniteOre").texture,3f,15000);
        Material painiteOre = new Material("PainiteOre",SpriteManager.GetSprite("PainiteOre").texture,1f,30000);
        
        BlockType dirtType = new BlockType(SpriteManager.GetSprite("DirtBlock").texture,0.3f,null,"DirtBreakEffect");
        BlockType grassType = new BlockType(SpriteManager.GetSprite("GrassBlock").texture,0.15f,null,"DirtBreakEffect");
        BlockType stoneType = new BlockType(SpriteManager.GetSprite("StoneBlock").texture,0.5f);
        BlockType coalType = new BlockType(SpriteManager.GetSprite("CoalBlock").texture,0.4f,coalOre);
        BlockType ironType = new BlockType(SpriteManager.GetSprite("IronBlock").texture,0.6f,ironOre);
        BlockType copperType = new BlockType(SpriteManager.GetSprite("CopperBlock").texture,0.6f,copperOre);
        BlockType silverType = new BlockType(SpriteManager.GetSprite("SilverBlock").texture,0.8f,silverOre);
        BlockType goldType = new BlockType(SpriteManager.GetSprite("GoldBlock").texture,0.9f,goldOre);
        BlockType titaniumType = new BlockType(SpriteManager.GetSprite("TitaniumBlock").texture, 1.2f, titaniumOre);
        BlockType amethystType = new BlockType(SpriteManager.GetSprite("AmethystBlock").texture, 1f, amethystOre,"AmethystBreakEffect");
        BlockType platinumType = new BlockType(SpriteManager.GetSprite("PlatinumBlock").texture, 1.3f, platinumOre);
        BlockType sapphireType = new BlockType(SpriteManager.GetSprite("SapphireBlock").texture, 1.4f, sapphireOre);
        BlockType rubyType = new BlockType(SpriteManager.GetSprite("RubyBlock").texture,1.5f,rubyOre);
        BlockType emeraldType = new BlockType(SpriteManager.GetSprite("EmeraldBlock").texture,1.7f,emeraldOre);
        BlockType opalType = new BlockType(SpriteManager.GetSprite("OpalBlock").texture, 1.8f, opalOre,"OpalBreakEffect");
        BlockType diamondType = new BlockType(SpriteManager.GetSprite("DiamondBlock").texture,2.2f,diamondOre);
        BlockType kryptoniteType = new BlockType(SpriteManager.GetSprite("KryptoniteBlock").texture, 2.5f, kryptoniteOre,"KryptoniteBreakEffect");
        BlockType painiteType = new BlockType(SpriteManager.GetSprite("PainiteBlock").texture, 2.5f, painiteOre);
        BlockType obsidianType = new BlockType(SpriteManager.GetSprite("ObsidianBlock").texture, 3f,null,"ObsidianBreakEffect");
        
        zones.Add(new Zone(0, 0, new Dictionary<BlockType, float>{{grassType,1f}},dirtType)); //0
        zones.Add(new Zone(1, 2, new Dictionary<BlockType, float>{{dirtType,1f}},dirtType)); //1
        zones.Add(new Zone(3, 50, new Dictionary<BlockType, float>{{stoneType,0.15f},{ironType,0.1f},{coalType,0.12f},{copperType,0.08f}},dirtType)); //2
        zones.Add(new Zone(51, 100, new Dictionary<BlockType, float>{{stoneType,0.18f},{ironType,0.12f},{coalType,0.08f},{copperType,0.1f},{silverType,0.04f}},dirtType)); //3
        zones.Add(new Zone(101, 150, new Dictionary<BlockType, float>{{ironType,0.1f},{copperType,0.08f},{goldType,0.04f},{silverType,0.07f},{titaniumType,0.05f}},stoneType)); //4
        zones.Add(new Zone(151, 200, new Dictionary<BlockType, float>{{ironType,0.08f},{coalType,0.1f},{copperType,0.1f},{goldType,0.06f},{silverType,0.08f},{titaniumType,0.06f},{amethystType,0.04f}},stoneType));//5
        zones.Add(new Zone(201, 250, new Dictionary<BlockType, float>{{silverType,0.06f},{goldType,0.07f},{titaniumType,0.05f},{amethystType,0.06f},{sapphireType,0.03f},{platinumType,0.03f}},stoneType));//6
        zones.Add(new Zone(251, 300, new Dictionary<BlockType, float>{{obsidianType,0.01f},{goldType,0.06f},{amethystType,0.06f},{sapphireType,0.05f},{rubyType,0.03f},{platinumType,0.04f}},stoneType));//7
        zones.Add(new Zone(301, 350, new Dictionary<BlockType, float>{{stoneType,0.05f},{platinumType,0.04f},{amethystType,0.05f},{sapphireType,0.06f},{rubyType,0.05f},{emeraldType,0.03f}},obsidianType));//8
        zones.Add(new Zone(351, 400, new Dictionary<BlockType, float>{{diamondType,0.02f},{opalType,0.03f},{stoneType,0.03f},{sapphireType,0.05f},{rubyType,0.06f},{emeraldType,0.05f}},obsidianType));//9
        zones.Add(new Zone(401, 450, new Dictionary<BlockType, float>{{diamondType,0.03f},{opalType,0.04f},{stoneType,0.01f},{platinumType,0.04f},{rubyType,0.06f},{emeraldType,0.06f}},obsidianType));//10
        zones.Add(new Zone(451, 500, new Dictionary<BlockType, float>{{kryptoniteType,0.02f},{diamondType,0.04f},{opalType,0.05f},{obsidianType,0.06f},{painiteType,0.02f},{emeraldType,0.03f}},obsidianType));//11
        
    }

    private void AddGameSprites()
    {
        SpriteManager.AddSprite("UnderGround","Images/BgTexture");
        SpriteManager.AddSprite("Sun","Images/Sun");
        SpriteManager.AddSprite("MenuBackground","Images/MenuBG");
        SpriteManager.AddSprite("Button1","Images/CrackedButton1");
        SpriteManager.AddSprite("CloseButton64","Images/XButton64");
        SpriteManager.AddSprite("Explosion","Images/ExplosionSheet",13,1);
        SpriteManager.AddSprite("DrillPod","Images/DrillPod");
        SpriteManager.AddSprite("Pixel","Images/Pixel");
        SpriteManager.AddSprite("DownDrill","Images/DrillDownSpriteSheet",5,1);
        SpriteManager.AddSprite("RightDrill","Images/DrillRightSpriteSheet",5,1);
        SpriteManager.AddSprite("LeftDrill","Images/DrillLeftSpriteSheet",5,1);
        SpriteManager.AddSprite("InventorySlots","Images/Inventory");
        SpriteManager.AddSprite("BarFill","Images/BarFill");
        SpriteManager.AddSprite("Bar","Images/Bar");
        SpriteManager.AddSprite("OilIcon","Images/OilIcon");
        SpriteManager.AddSprite("HealthIcon","Images/HealthIcon");
        SpriteManager.AddSprite("Flame","Images/FlameSheet",4,1);
        SpriteManager.AddSprite("GasStation","Shops/GasStation");
        SpriteManager.AddSprite("MineralsShop","Shops/MineralsShop");
        SpriteManager.AddSprite("RepairStation","Shops/RepairShop");
        SpriteManager.AddSprite("UpgradesShop","Shops/UpgradesShop");
        SpriteManager.AddSprite("ItemsShop","Shops/ItemsShop");
        SpriteManager.AddSprite("Panel1","Images/Panel1");
        SpriteManager.AddSprite("Drill","Images/DrillRight");
        SpriteManager.AddSprite("Cargo","Images/CargoIcon");
        SpriteManager.AddSprite("Hull","Images/HullIcon");
        SpriteManager.AddSprite("Portal","Images/Dimensional_Portal",3,2);
        SpriteManager.AddSprite("Cloud1","Images/cloud1");
        SpriteManager.AddSprite("Cloud2","Images/cloud2");
        SpriteManager.AddSprite("Cloud3","Images/cloud3");
        SpriteManager.AddSprite("BreakEffect","Images/BreakEffect",6,1);
        SpriteManager.AddSprite("DirtBreakEffect","Images/DirtBreakEffect",6,1);
        SpriteManager.AddSprite("AmethystBreakEffect","Images/AmethystBreakEffect",6,1);
        SpriteManager.AddSprite("KryptoniteBreakEffect","Images/KryptoniteBreakEffect",6,1);
        SpriteManager.AddSprite("OpalBreakEffect","Images/OpalBreakEffect",6,1);
        SpriteManager.AddSprite("ObsidianBreakEffect","Images/ObsidianBreakEffect",6,1);
        SpriteManager.AddSprite("LandEffect","Images/LandEffect",7,1);
        SpriteManager.AddSprite("BombEffect","Images/BombEffect",4,1);
        SpriteManager.AddSprite("Teleport","Items/Teleport");
        SpriteManager.AddSprite("Bomb","Items/Bomb");
        SpriteManager.AddSprite("RepairKit","Items/RepairKit");
        SpriteManager.AddSprite("FuelTank","Items/FuelTank");
        
        SpriteManager.AddSprite("DirtBlock","Blocks/DirtBlock");
        SpriteManager.AddSprite("GrassBlock","Blocks/GrassBlock");
        SpriteManager.AddSprite("StoneBlock","Blocks/StoneBlock");
        SpriteManager.AddSprite("IronBlock","Blocks/IronBlock");
        SpriteManager.AddSprite("DiamondBlock","Blocks/DiamondBlock");
        SpriteManager.AddSprite("GoldBlock","Blocks/GoldBlock");
        SpriteManager.AddSprite("EmeraldBlock","Blocks/EmeraldBlock");
        SpriteManager.AddSprite("CoalBlock","Blocks/CoalBlock");
        SpriteManager.AddSprite("CopperBlock","Blocks/CopperBlock");
        SpriteManager.AddSprite("RubyBlock","Blocks/RubyBlock");
        SpriteManager.AddSprite("SilverBlock","Blocks/SilverBlock");
        SpriteManager.AddSprite("SapphireBlock","Blocks/SapphireBlock");
        SpriteManager.AddSprite("TitaniumBlock","Blocks/TitaniumBlock");
        SpriteManager.AddSprite("AmethystBlock","Blocks/AmethystBlock");
        SpriteManager.AddSprite("KryptoniteBlock","Blocks/KryptoniteBlock");
        SpriteManager.AddSprite("PlatinumBlock","Blocks/PlatinumBlock");
        SpriteManager.AddSprite("OpalBlock","Blocks/OpalBlock");
        SpriteManager.AddSprite("PainiteBlock","Blocks/PainiteBlock");
        SpriteManager.AddSprite("ObsidianBlock","Blocks/ObsidianBlock");
        SpriteManager.AddSprite("BedrockBlock","Blocks/BedrockBlock");
        
        SpriteManager.AddSprite("IronOre","Ores/IronOre");
        SpriteManager.AddSprite("DiamondOre","Ores/DiamondOre");
        SpriteManager.AddSprite("GoldOre","Ores/GoldOre");
        SpriteManager.AddSprite("CoalOre","Ores/CoalOre");
        SpriteManager.AddSprite("RubyOre","Ores/RubyOre");
        SpriteManager.AddSprite("SilverOre","Ores/SilverOre");
        SpriteManager.AddSprite("CopperOre","Ores/CopperOre");
        SpriteManager.AddSprite("EmeraldOre","Ores/EmeraldOre");
        SpriteManager.AddSprite("SapphireOre","Ores/SapphireOre");
        SpriteManager.AddSprite("TitaniumOre","Ores/TitaniumOre");
        SpriteManager.AddSprite("AmethystOre","Ores/AmethystOre");
        SpriteManager.AddSprite("KryptoniteOre","Ores/KryptoniteOre");
        SpriteManager.AddSprite("PlatinumOre","Ores/PlatinumOre");
        SpriteManager.AddSprite("OpalOre","Ores/OpalOre");
        SpriteManager.AddSprite("PainiteOre","Ores/PainiteOre");
    }

    private void AddAudio()
    {
        AudioManager.AddSong("song1","Audio/Music/song1");
        AudioManager.AddSong("song2","Audio/Music/song2");
        AudioManager.AddSong("song3","Audio/Music/song3");
        AudioManager.AddSong("song4","Audio/Music/song4");
        AudioManager.AddSong("song5","Audio/Music/song5");
        AudioManager.AddSong("song6","Audio/Music/song6");
        AudioManager.AddSoundEffect("portalSound", "Audio/SFX/portalSound");
        AudioManager.AddSoundEffect("portalEnter", "Audio/SFX/portalEnterSfx");
        AudioManager.AddSoundEffect("drillSfx", "Audio/SFX/drillSfx");
        AudioManager.AddSoundEffect("fuelWarningSfx", "Audio/SFX/lowFuelSfx");
        AudioManager.AddSoundEffect("explosion","Audio/SFX/explosion");
        AudioManager.AddSoundEffect("Thrust", "Audio/SFX/thrust");
        AudioManager.AddSoundEffect("Impact", "Audio/SFX/impact");
        AudioManager.AddSoundEffect("BombExplode", "Audio/SFX/BombExplodeSound");
        AudioManager.AddSoundEffect("ButtonClick", "Audio/SFX/ButtonClick");
        AudioManager.AddSoundEffect("ButtonHoverSound", "Audio/SFX/ButtonHoverSound");
        AudioManager.AddSoundEffect("ErrorSound", "Audio/SFX/ErrorSound");
        AudioManager.AddSoundEffect("CollectMoneySound", "Audio/SFX/CollectMoneySound");
        AudioManager.AddSoundEffect("UpgradeSound", "Audio/SFX/UpgradeSound");
        AudioManager.AddSoundEffect("RefuelSound", "Audio/SFX/refuelSound");
    }
}