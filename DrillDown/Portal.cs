using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DrillDown;

public class Portal : IDrawable, IUpdatable
{
    protected Player player;
    private Animation portalSprite;
    private Rectangle entranceBounds;
    private int entranceWidth = 20;
    private int entranceHeight = 100;
    private bool wasEPressed;
    private bool playerIsInside;
    private Text promptText;
    private float scale;
    private Random random = new Random();
    private SoundEffectInstance portalSound;
    float maxRange = 500f;
    public Rectangle EntranceBounds => entranceBounds;

    public Portal(string spriteName, float scale, Player player)
    {
        this.scale = scale;
        this.player = player;
        portalSprite = new Animation(spriteName);
        portalSprite.PlayAnimation(true,10);
        portalSprite.anchor = Anchor.TopLeft;
        portalSprite.tm.scale = new Vector2(scale, scale);
        portalSprite.sortingOrder = 1f;
        portalSound = AudioManager.CreateSfxInstance("portalSound",0f);
        portalSound.Play();
    }
    
    public void Start()
    {
    }
    
    private void SetPortalEntranceBounds(float scale)
    {
        Rectangle frame = portalSprite.spriteSheet[0, 0];
        int frameWidth = (int)(frame.Width * scale);
        float frameHeight = (int)(frame.Height * scale);
        float portalX = portalSprite.tm.position.X;
        float portalY = portalSprite.tm.position.Y;
        entranceBounds = new Rectangle(
            (int)(portalX + frameWidth * 0.5f - entranceWidth * 0.5f),
            (int)(portalY + frameHeight * 0.5f - entranceHeight * 0.5f),
            entranceWidth,
            entranceHeight
        );
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        portalSprite.Draw(spriteBatch);
    }

    

    public void Update(GameTime gameTime)
    {
        bool ePressed = Keyboard.GetState().IsKeyDown(Keys.E);
        playerIsInside = IsPlayerInside();
        if (playerIsInside && ePressed && !wasEPressed)
        {
            Console.WriteLine("Entered Portal");
            AudioManager.PlaySoundEffect("portalEnter");
            portalSound.Stop();
            GameManager.Instance.WinGame();
        }
        wasEPressed = ePressed;
        portalSprite.Update(gameTime);
        if(portalSound!=null)
            portalSound.Volume = ComputeVolumeByDistance();
    }
    
    public void DrawPrompt(SpriteBatch spriteBatch)
    {
        if (!playerIsInside) return;
        promptText.DrawTextBackground(spriteBatch);
        promptText.Draw(spriteBatch);
    }
    
    private void SetPromptText()
    {
        promptText = new Text
        {
            text = "Press E to Enter",
            font = Game1._font,
            color = Color.White,
            sortingOrder = 1f
        };
        promptText.tm.scale = new Vector2(0.6f, 0.6f);
        promptText.tm.position = new Vector2(entranceBounds.X, entranceBounds.Y);
    }

    private bool IsPlayerInside()
    {
        return player.destRect.Intersects(entranceBounds);
    }

    public void PlaceAtRandomPosition()
    {
        int bottomRow = Game1.rows - 2;
        int randomColumn = random.Next(0, Game1.columns);
        Console.WriteLine("Random Column: " + randomColumn);
        portalSprite.tm.position = new Vector2(Game1.groundLevel.X + randomColumn * Game1.blockSize, Game1.groundLevel.Y + bottomRow * Game1.blockSize);
        SetPortalEntranceBounds(scale);
        SetPromptText();
    }
    

    private float ComputeVolumeByDistance()
    {
        float distance = Vector2.Distance(player.tm.position, portalSprite.tm.position);
        return Math.Clamp(1f - distance / maxRange, 0f, 1f);
    }
}