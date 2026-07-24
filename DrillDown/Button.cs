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

    public Button(Sprite sprite, Vector2 position, int width, int height,float layer = 0.9f)//constructor for a button with texture
    {
        this.sprite = sprite;
        tintColor = Color.White;
        hoverColor = Color.Gray;
        this.position = position;
        this.width = width;
        this.height = height;
        sortingOrder = layer;
    }

    public Button(Color color, Vector2 position, int width, int height, float layer = 0.9f)//constructor for a button without texture
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
        if (isPressedNow && !isButtonPressed && isInside)//if button pressed in this frame
        {
            OnClick?.Invoke();//launch OnClick event
        }
        isButtonPressed = isPressedNow;
    }
        
    public void Draw(SpriteBatch spriteBatch)//draw the button either with texture or without
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

    public void SetText(string label, SpriteFont font, Color color,float layer = 1f ,float scale = 1f)//method to set text for the button, can change font, color and scale ...
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
}