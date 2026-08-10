using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace DrillDown;

public class Button : IUpdatable, IDrawable
{
    private readonly Sprite sprite;
    private readonly Vector2 position;
    private readonly int width, height;
    private readonly Color tintColor;
    private readonly Color hoverColor;
    public static Texture2D Pixel;
    private Rectangle bounds;
    private bool isButtonPressed;
    private Text text;
    public event Action OnClick;
    private readonly float sortingOrder;
    private bool isInside;
    private bool wasInside;

    public Button(Sprite sprite, Vector2 position, int width, int height,float layer = 0.9f)
    {
        this.sprite = sprite;
        tintColor = Color.White;
        hoverColor = Color.Gray;
        this.position = position;
        this.width = width;
        this.height = height;
        sortingOrder = layer;
    }

    public Button(Color color, Vector2 position, int width, int height, float layer = 0.9f)
    {
        sprite = null;
        tintColor = color;
        hoverColor = Color.Gray;
        this.position = position;
        this.width = width;
        this.height = height;
        sortingOrder = layer;
    }

    public Button()
    {
    }

    public void Start()
    {
        bounds = new Rectangle((int)position.X, (int)position.Y, width, height);
    }

    public void Update(GameTime gameTime)
    {
        MouseState mouseState = Mouse.GetState();
        bool isPressedNow = mouseState.LeftButton == ButtonState.Pressed;
        isInside = bounds.Contains(mouseState.Position);
        if (isInside&&!wasInside)
            AudioManager.PlaySoundEffect("ButtonHoverSound",false,0.3f);
        if (isPressedNow && !isButtonPressed && isInside)
        {
            OnClick?.Invoke();
            AudioManager.PlaySoundEffect("ButtonClick",false,0.3f);
        }
        isButtonPressed = isPressedNow;
        wasInside = isInside;
    }
        
    public void Draw(SpriteBatch spriteBatch)
    {
        Texture2D drawTexture = sprite.texture ?? Pixel;
        Color drawColor = isInside ? hoverColor : tintColor;
        spriteBatch.Draw(
            drawTexture,
            bounds, 
            null ,
            drawColor,
            0f,
            Vector2.Zero,
            SpriteEffects.None,
            sortingOrder);
        text?.Draw(spriteBatch);
    }

    public void SetText(string label, SpriteFont font, Color color,float layer = 1f ,float scale = 1f)
    {
        text = new Text
        {
            tm =
            {
                scale = new Vector2(scale, scale),
                position = position + new Vector2(width / 2f, height / 2f)
            },
            text = label,
            font = font,
            color = color,
            sortingOrder = layer
        };
    }
    public void SetTextAtPos(string label, SpriteFont font, Color color,Vector2 pos,float layer = 1f ,float scale = 1f)
    {
        text = new Text
        {
            tm =
            {
                scale = new Vector2(scale, scale),
                position = pos
            },
            text = label,
            font = font,
            color = color,
            sortingOrder = layer
        };
    }
}