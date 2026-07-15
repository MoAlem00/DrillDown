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
    private int blockSize = 64;
    private int columns = 30;
    private int rows = 15;
    private Player player;
    private Sprite backGround;
    private SpriteFont _font;
    private Button startButton, settingsButton, quitButton;
    private int buttonWidth = 200;
    private int buttonHeight = 80;
    private Vector2 buttonsOffset = new Vector2(0,100);
    private Vector2 buttonsCentered;
    //private List<Sprite> blocks = new();
    public static Vector2 groundLevel;
    private Vector2 yLevelOffset = new Vector2(0, 200f);
    private SpriteManager spriteManager;
    private WorldGenerator worldGenerator;
    private Block[,] grid;
    private World world;
    private Camera camera;
    //Matrix transform;

    
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
        Texture2D buttonTexture =  Content.Load<Texture2D>("Images/CrackedButton");
        
        Button.Pixel = new Texture2D(GraphicsDevice, 1, 1);
        Button.Pixel.SetData(new Color[] { Color.White });
        
        SpriteManager.AddSprite("BackGround","Images/BG");
        SpriteManager.AddSprite("DirtBlock","Images/Dirt");
        SpriteManager.AddSprite("GrassBlock","Images/Grass");
        SpriteManager.AddSprite("StoneBlock","Images/Stone");
        SpriteManager.AddSprite("DrillPod","Images/DrillPod");
        SpriteManager.AddSprite("Pixel","Images/Pixel");
        SpriteManager.AddSprite("IronBlock","Images/IronBlock");
        SpriteManager.AddSprite("DiamondBlock","Images/DiamondBlock");
        SpriteManager.AddSprite("GoldBlock","Images/GoldBlock");
        
        Material ironOre = new Material("Iron Ore",Content.Load<Texture2D>("Images/IronOre"),5f,50);
        
        BlockType dirtType = new BlockType("Dirt",SpriteManager.GetSprite("DirtBlock").texture,1f);
        BlockType stoneType = new BlockType("Stone",SpriteManager.GetSprite("StoneBlock").texture,2f);
        BlockType grassType = new BlockType("Grass",SpriteManager.GetSprite("GrassBlock").texture,1f);
        BlockType ironType = new BlockType("Iron",SpriteManager.GetSprite("IronBlock").texture,5f,ironOre);
        BlockType goldType = new BlockType("Gold",SpriteManager.GetSprite("GoldBlock").texture,1f);
        BlockType diamondType = new BlockType("Diamond",SpriteManager.GetSprite("DiamondBlock").texture,1f);
        List<BlockType> blockTypes = new List<BlockType>();
        blockTypes.Add(grassType);
        blockTypes.Add(dirtType);
        blockTypes.Add(stoneType);
        blockTypes.Add(ironType);
        blockTypes.Add(goldType);
        blockTypes.Add(diamondType);
        
        worldGenerator = new WorldGenerator(rows, columns, blockTypes);
        grid = worldGenerator.GenerateWorld();

        world = new World(grid, blockSize, groundLevel);
        
        
        backGround = new Sprite("BackGround");
        backGround.sortingOrder = 0f / totalLayers;
        
        buttonsCentered = _screenCenter - new Vector2(buttonWidth / 2f, buttonHeight / 2f);
        float textLayer = 5f / totalLayers;
        startButton = new Button(buttonTexture, buttonsCentered, buttonWidth, buttonHeight);
        startButton.SetText("Start", _font, Color.White,textLayer);
        
        settingsButton = new Button(buttonTexture,buttonsCentered + buttonsOffset, buttonWidth, buttonHeight);
        settingsButton.SetText("Settings", _font, Color.White,textLayer);
        
        quitButton = new Button(buttonTexture,buttonsCentered + buttonsOffset*2, buttonWidth, buttonHeight);
        quitButton.SetText("Quit", _font, Color.White,textLayer);
        
        
        startButton.OnClick += gameManager.StartGame;
        settingsButton.OnClick += () => Console.WriteLine("Settings");
        gameManager.OnGameStart += () => IsMouseVisible = false;
        quitButton.OnClick += Exit;
        
        Start();
        
    }
    private void Start()
    {
        player = SceneManager.Create<Player>();
        player.world = world;
        player.PlayAnimation();
        player.sortingOrder = 4f / totalLayers;
        
        player.collider.RegisterOnCollision(player.OnCollision);
        player.collider.RegisterOnTrigger(player.OnTrigger);
        
        startButton.Start();
        settingsButton.Start();
        quitButton.Start();

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
                break;
        }
        SceneManager.Instance.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Brown);

        _spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp, transformMatrix: camera.position);
        backGround.Draw(_spriteBatch);
        world.Draw(_spriteBatch);
        switch (gameManager.gameState)
        {
            case GameManager.GameState.MainMenu:
                startButton.Draw(_spriteBatch);
                settingsButton.Draw(_spriteBatch);
                quitButton.Draw(_spriteBatch);
                break;
            case GameManager.GameState.Playing:
                player.Draw(_spriteBatch);
                _spriteBatch.Draw(Button.Pixel, player.destRect, Color.White * 0.9f);
                int col = (int)((player.tm.position.X - groundLevel.X) / blockSize);
                int row = (int)((player.tm.position.Y - groundLevel.Y) / blockSize);
                _spriteBatch.Draw(Button.Pixel, world.CellRect(row, col), Color.Red * 0.9f);
                break;
        }
        SceneManager.Instance.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}