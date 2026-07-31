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
    private int totalLayers = 5;
    public static Vector2 _screenCenter;
    public static Vector2 _screenLeftCenter;
    public static Vector2 _screenRightCorner;
    public static Vector2 _screenLeftCorner;
    public static Vector2 _screenTopCenter;
    public static int _screenWidth;
    public const int blockSize = 64;
    private int columns = 55;
    private int rows = 70;
    private Player player;
    public static SpriteFont _font;
    private List<Button> menuButtons = new();
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
    private MainMenu mainMenu;
    private List<Shop> shops = new();

    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        spriteManager = new SpriteManager(Content);
        
        
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
        
        
        //SpriteManager.AddSprite("BackGround","Images/BG");
        SpriteManager.AddSprite("MenuBackground","Images/MenuBG");
        //SpriteManager.AddSprite("Button","Images/CrackedButton");
        SpriteManager.AddSprite("Button1","Images/CrackedButton1");
        //SpriteManager.AddSprite("CloseButton","Images/XButton");
        //SpriteManager.AddSprite("CloseButton32","Images/XButton32");
        SpriteManager.AddSprite("CloseButton64","Images/XButton64");
        SpriteManager.AddSprite("Explosion","Images/ExplosionSheet",13,1);
        //SpriteManager.AddSprite("EarthBackground","Images/EarthBackground");
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
        //SpriteManager.AddSprite("Panel","Images/Panel");
        SpriteManager.AddSprite("Panel1","Images/Panel1");
        
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
        Material emeraldOre = new Material("EmeraldOre",SpriteManager.GetSprite("EmeraldOre").texture,1f,1000000);
        Material coalOre = new Material("CoalOre",SpriteManager.GetSprite("CoalOre").texture,1f,2000);
        Material copperOre = new Material("CopperOre",SpriteManager.GetSprite("CopperOre").texture,1f,2000);
        Material rubyOre = new Material("RubyOre",SpriteManager.GetSprite("RubyOre").texture,1f,2000);
        Material silverOre = new Material("SilverOre",SpriteManager.GetSprite("SilverOre").texture,1f,2000);
        
        BlockType dirtType = new BlockType("Dirt",SpriteManager.GetSprite("DirtBlock").texture,0.2f);
        BlockType stoneType = new BlockType("Stone",SpriteManager.GetSprite("StoneBlock").texture,0.3f);
        BlockType grassType = new BlockType("Grass",SpriteManager.GetSprite("GrassBlock").texture,0.2f);
        BlockType ironType = new BlockType("Iron",SpriteManager.GetSprite("IronBlock").texture,0.4f,ironOre);
        BlockType goldType = new BlockType("Gold",SpriteManager.GetSprite("GoldBlock").texture,0.35f,goldOre);
        BlockType diamondType = new BlockType("Diamond",SpriteManager.GetSprite("DiamondBlock").texture,0.5f,diamondOre);
        BlockType emeraldType = new BlockType("Emerald",SpriteManager.GetSprite("EmeraldBlock").texture,0.5f,emeraldOre);
        BlockType coalType = new BlockType("Coal",SpriteManager.GetSprite("CoalBlock").texture,0.32f,coalOre);
        BlockType rubyType = new BlockType("Ruby",SpriteManager.GetSprite("RubyBlock").texture,0.5f,rubyOre);
        BlockType silverType = new BlockType("Silver",SpriteManager.GetSprite("SilverBlock").texture,0.5f,silverOre);
        BlockType copperType = new BlockType("Copper",SpriteManager.GetSprite("CopperBlock").texture,0.32f,copperOre);
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
        camera = new Camera(world);

        Start();
    }
    private void Start()
    {   
        player = new Player();
        hud = new HUD(player.Inventory,_font);
        player.world = world;
        player.PlayAnimation();
        player.sortingOrder = 0.99f; //3f / totalLayers;
        player.OnFuelChange += hud.HandleFuelChange;
        player.OnHealthChange += hud.HandleHealthChange;
        player.OnMoneyChange += hud.HandleMoneyChange;
        player.OnPlayerDeath += GameManager.Instance.HandleGameOver;
        GameManager.Instance.OnQuitGame += Exit;
        player.Start();
        shops.Add(new RepairStation("RepairStation", 0.3f, 35,player));
        shops.Add(new MineralsShop("MineralsShop",0.5f,20,player));
        shops.Add(new GasStation("GasStation",0.4f, 5,player));
        shops.Add(new UpgradesShop("UpgradesShop",0.3f, 50,player));
        MakeBlocksBelowShopsUnbreakable(shops);
        mainMenu = new MainMenu(new Sprite("MenuBackground"));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        camera.Follow(player);
        switch (GameManager.Instance.gameState)
        {
            case GameManager.GameState.MainMenu:
                mainMenu.Update(gameTime);
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
                Console.WriteLine("GameOver");
                break;
        }
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.SaddleBrown);

        _spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp,
            transformMatrix: camera.position);
        //backGround.Draw(_spriteBatch);
        
        world.Draw(_spriteBatch);
        if (GameManager.Instance.gameState == GameManager.GameState.Playing)
        {
            player.Draw(_spriteBatch);
            foreach (var shop in shops)
            {
                shop.Draw(_spriteBatch);
                shop.DrawPrompt(_spriteBatch);
            }
        }

        if (GameManager.Instance.gameState == GameManager.GameState.GameOver)
        {
            //player.Draw(_spriteBatch);
            //Console.WriteLine("GameOver");
        }
        _spriteBatch.End();
        
        _spriteBatch.Begin();
        hud.Draw(_spriteBatch);
        if (GameManager.Instance.gameState == GameManager.GameState.Playing)
        {
            foreach (var shop in shops)
            {
                shop.DrawPanel(_spriteBatch);
            }
        }
        
        if (GameManager.Instance.gameState == GameManager.GameState.MainMenu)
        {
            mainMenu.Draw(_spriteBatch);
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
                world.SetBlockUnbreakable(0,c);
        }
    }
    
}