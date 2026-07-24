using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DrillDown;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private GameManager gameManager;
    private int totalLayers = 5;
    public static Vector2 _screenCenter;
    public static Vector2 _screenLeftCenter;
    public static Vector2 _screenRightCorner;
    public static Vector2 _screenLeftCorner;
    public static Vector2 _screenTopCenter;
    public static int _screenWidth;
    public const int blockSize = 64;
    private int columns = 50;
    private int rows = 50;
    private Player player;
    private Sprite backGround;
    private Sprite bottomBackGround;
    public static SpriteFont _font;
    //public static SpriteFont _titleFont;
    private Button startButton, settingsButton, quitButton;
    private int buttonWidth = 200;
    private int buttonHeight = 80;
    private Vector2 buttonsOffset = new Vector2(0,100);
    private Vector2 buttonsCentered;
    public static Vector2 groundLevel;
    private Vector2 yLevelOffset = new Vector2(0, 200f);
    private SpriteManager spriteManager;
    private WorldGenerator worldGenerator;
    private Block[,] grid;
    private World world;
    private Camera camera;
    private HUD hud;
    private List<Shop> shops = new();
    /*private Shop gasStation;
    private Shop mineralsShop;
    private Shop repairStation;*/

    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        spriteManager = new SpriteManager(Content);
        gameManager = new GameManager();
        camera = new Camera();
        
        
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
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        groundLevel = _screenLeftCenter + yLevelOffset;
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("Fonts/GeistPixel");
        
        Button.Pixel = new Texture2D(GraphicsDevice, 1, 1);
        Button.Pixel.SetData(new Color[] { Color.White });
        
        
        SpriteManager.AddSprite("BackGround","Images/BG");
        SpriteManager.AddSprite("Button","Images/CrackedButton");
        SpriteManager.AddSprite("Button1","Images/CrackedButton1");
        SpriteManager.AddSprite("CloseButton","Images/XButton");
        SpriteManager.AddSprite("CloseButton32","Images/XButton32");
        SpriteManager.AddSprite("CloseButton64","Images/XButton64");
        SpriteManager.AddSprite("Explosion","Images/ExplosionSheet",13,1);
        SpriteManager.AddSprite("EarthBackground","Images/EarthBackground");
        SpriteManager.AddSprite("DrillPod","Images/DrillPod");
        SpriteManager.AddSprite("Pixel","Images/Pixel");
        SpriteManager.AddSprite("DownDrill","Images/DrillDownSpriteSheet",5,1);
        SpriteManager.AddSprite("RightDrill","Images/DrillRightSpriteSheet",5,1);
        SpriteManager.AddSprite("LeftDrill","Images/DrillLeftSpriteSheet",5,1);
        SpriteManager.AddSprite("InventorySlots","Images/Inventory");
        SpriteManager.AddSprite("BarFill","Images/BarFill");
        SpriteManager.AddSprite("Bar","Images/Bar");
        SpriteManager.AddSprite("FuelIcon","Images/FuelIcon");
        SpriteManager.AddSprite("HealthIcon","Images/HealthIcon");
        SpriteManager.AddSprite("Flame","Images/FlameSheet",4,1);
        SpriteManager.AddSprite("GasStation","Shops/GasStation");
        SpriteManager.AddSprite("MineralsShop","Shops/MineralsShop");
        SpriteManager.AddSprite("RepairStation","Shops/RepairShop");
        SpriteManager.AddSprite("UpgradesShop","Shops/UpgradesShop");
        SpriteManager.AddSprite("Panel","Images/Panel");
        
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
        
        SpriteManager.AddSprite("IronOre","Ores/IronOre");
        SpriteManager.AddSprite("DiamondOre","Ores/DiamondOre");
        SpriteManager.AddSprite("GoldOre","Ores/GoldOre");
        SpriteManager.AddSprite("CoalOre","Ores/CoalOre");
        SpriteManager.AddSprite("RubyOre","Ores/RubyOre");
        SpriteManager.AddSprite("SilverOre","Ores/SilverOre");
        SpriteManager.AddSprite("CopperOre","Ores/CopperOre");
        SpriteManager.AddSprite("EmeraldOre","Ores/EmeraldOre");
        
        
        Material ironOre = new Material("IronOre",SpriteManager.GetSprite("IronOre").texture,1f,50);
        Material goldOre = new Material("GoldOre",SpriteManager.GetSprite("GoldOre").texture,1f,500);
        Material diamondOre = new Material("DiamondOre",SpriteManager.GetSprite("DiamondOre").texture,1f,1000);
        Material emeraldOre = new Material("EmeraldOre",SpriteManager.GetSprite("EmeraldOre").texture,1f,2000);
        Material coalOre = new Material("CoalOre",SpriteManager.GetSprite("CoalOre").texture,1f,2000);
        Material copperOre = new Material("CopperOre",SpriteManager.GetSprite("CopperOre").texture,1f,2000);
        Material rubyOre = new Material("RubyOre",SpriteManager.GetSprite("RubyOre").texture,1f,2000);
        Material silverOre = new Material("SilverOre",SpriteManager.GetSprite("SilverOre").texture,1f,2000);
        
        BlockType dirtType = new BlockType("Dirt",SpriteManager.GetSprite("DirtBlock").texture,0.2f);
        BlockType stoneType = new BlockType("Stone",SpriteManager.GetSprite("StoneBlock").texture,0.2f);
        BlockType grassType = new BlockType("Grass",SpriteManager.GetSprite("GrassBlock").texture,0.2f);
        BlockType ironType = new BlockType("Iron",SpriteManager.GetSprite("IronBlock").texture,0.2f,ironOre);
        BlockType goldType = new BlockType("Gold",SpriteManager.GetSprite("GoldBlock").texture,0.2f,goldOre);
        BlockType diamondType = new BlockType("Diamond",SpriteManager.GetSprite("DiamondBlock").texture,0.2f,diamondOre);
        BlockType emeraldType = new BlockType("Emerald",SpriteManager.GetSprite("EmeraldBlock").texture,0.2f,emeraldOre);
        BlockType coalType = new BlockType("Coal",SpriteManager.GetSprite("CoalBlock").texture,0.2f,coalOre);
        BlockType rubyType = new BlockType("Ruby",SpriteManager.GetSprite("RubyBlock").texture,0.2f,rubyOre);
        BlockType silverType = new BlockType("Silver",SpriteManager.GetSprite("SilverBlock").texture,0.2f,silverOre);
        BlockType copperType = new BlockType("Copper",SpriteManager.GetSprite("CopperBlock").texture,0.2f,copperOre);
        List<BlockType> blockTypes = new List<BlockType>();
        blockTypes.Add(grassType);
        blockTypes.Add(dirtType);
        blockTypes.Add(stoneType);
        blockTypes.Add(ironType);
        blockTypes.Add(goldType);
        blockTypes.Add(diamondType);
        blockTypes.Add(emeraldType);
        blockTypes.Add(coalType);
        blockTypes.Add(rubyType);
        blockTypes.Add(silverType);
        blockTypes.Add(copperType);
        
        worldGenerator = new WorldGenerator(rows, columns, blockTypes);
        grid = worldGenerator.GenerateWorld();

        world = new World(grid, blockSize, groundLevel, 4f/totalLayers);
        
        
        backGround = new Sprite("BackGround");
        bottomBackGround = new Sprite("EarthBackground");
        backGround.sortingOrder = 0f / totalLayers;

        Sprite button = new Sprite("Button");
        buttonsCentered = _screenCenter - new Vector2(buttonWidth / 2f, buttonHeight / 2f);
        float textLayer = 5f / totalLayers;
        startButton = new Button(button, buttonsCentered, buttonWidth, buttonHeight);
        startButton.SetText("Start", _font, Color.White,textLayer);
        
        settingsButton = new Button(button,buttonsCentered + buttonsOffset, buttonWidth, buttonHeight);
        settingsButton.SetText("Settings", _font, Color.White,textLayer);
        
        quitButton = new Button(button,buttonsCentered + buttonsOffset*2, buttonWidth, buttonHeight);
        quitButton.SetText("Quit", _font, Color.White,textLayer);
        
        
        startButton.OnClick += gameManager.StartGame;
        settingsButton.OnClick += () => Console.WriteLine("Settings");
        //gameManager.OnGameStart += () => IsMouseVisible = false;
        quitButton.OnClick += Exit;
        Start();
        
    }
    private void Start()
    {
        player = SceneManager.Create<Player>();
        hud = new HUD(player.Inventory,_font);
        player.world = world;
        player.PlayAnimation();
        player.sortingOrder = 0.99f; //3f / totalLayers;
        player.collider.RegisterOnCollision(player.OnCollision);
        player.collider.RegisterOnTrigger(player.OnTrigger);
        player.OnFuelChange += hud.HandleFuelChange;
        player.OnHealthChange += hud.HandleHealthChange;
        player.OnMoneyChange += hud.HandleMoneyChange;
        player.OnPlayerDeath += gameManager.HandleGameOver;
        startButton.Start();
        settingsButton.Start();
        quitButton.Start();
        shops.Add(new RepairStation("RepairStation", 0.3f, 65,player));
        shops.Add(new MineralsShop("MineralsShop",0.5f,45,player));
        shops.Add(new GasStation("GasStation",0.4f, 5,player));
        //shops.Add(new GasStation("UpgradesShop",0.3f, 25,player));
        MakeBlocksBelowShopsUnbreakable(shops);
        SceneManager.Instance.Start();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        camera.Follow(player);
        switch (gameManager.gameState)
        {
            case GameManager.GameState.MainMenu:
                startButton.Update(gameTime);
                settingsButton.Update(gameTime);
                quitButton.Update(gameTime);
                break;
            case GameManager.GameState.Playing:
                player.Update(gameTime);
                foreach (var shop in shops)
                {
                    shop.Update();
                    shop.UpdatePanel(gameTime);
                }
                break;
            case GameManager.GameState.GameOver:
                player.Update(gameTime);
                break;
        }
        SceneManager.Instance.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.SaddleBrown);

        _spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp,
            transformMatrix: camera.position);
        backGround.Draw(_spriteBatch);
        world.Draw(_spriteBatch);
        
        if (gameManager.gameState == GameManager.GameState.Playing)
        {
            player.Draw(_spriteBatch);
            /*int col = (int)((player.tm.position.X - groundLevel.X) / blockSize);
            int row = (int)((player.tm.position.Y - groundLevel.Y) / blockSize);
            _spriteBatch.Draw(Button.Pixel, world.CellRect(row, col), Color.Red * 0.9f);*/
            SceneManager.Instance.Draw(_spriteBatch);
            foreach (var shop in shops)
            {
                shop.Draw(_spriteBatch);
                shop.DrawPrompt(_spriteBatch);
            }
        }

        if (gameManager.gameState == GameManager.GameState.GameOver)
        {
            player.Draw(_spriteBatch);
        }
        _spriteBatch.End();
        
        _spriteBatch.Begin();
        hud.Draw(_spriteBatch);
        foreach (var shop in shops)
        {
            shop.DrawPanel(_spriteBatch);
        }

        if (gameManager.gameState == GameManager.GameState.MainMenu)
        {
            startButton.Draw(_spriteBatch);
            settingsButton.Draw(_spriteBatch);
            quitButton.Draw(_spriteBatch);
        }
        _spriteBatch.End();
        base.Draw(gameTime);
    }

    private void MakeBlocksBelowShopsUnbreakable(List<Shop> shops)
    {
        foreach (Shop shop in shops)
        {
            Vector2 shopPos = shop.GetShopPosition();
            int blocksCovered = shop.GetShopWidthInBlocks();
            int shopStartingCol = world.WorldToCol(shopPos);
            for (int c = shopStartingCol ; c < blocksCovered + shopStartingCol; c++)
            {
                Console.WriteLine(c);
                world.SetBlockUnbreakable(0,c);
            }
        }
    }
}