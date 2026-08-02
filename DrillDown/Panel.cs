using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class Panel
{
    private Sprite panelSprite;
    private Text titleText;
    private int cols, rows;
    private int cellH, cellW;
    private float scale;
    private Sprite buttonSprite;
    private List<Button> buttons = new();
    private int padding = 20;
    private int titleOffsetY = 60;
    
    public Panel(Sprite panelSprite,int cols, int rows,float scale = 1.4f,int paddingY = 0)
    {
        panelSprite.tm.position = Game1._screenCenter - new Vector2((panelSprite.texture.Width * scale) /2, (panelSprite.texture.Height * scale - paddingY) / 2);
        panelSprite.tm.scale = new Vector2(scale, scale);
        this.panelSprite = panelSprite;
        this.scale = scale;
        this.cols = cols;
        this.rows = rows;
        int scaledWidth = (int)(panelSprite.texture.Width * scale);
        int scaledHeight = (int)(panelSprite.texture.Height * scale);
        cellH = scaledHeight / rows;
        cellW = scaledWidth / cols;
    }
    
    public void DrawGridDebug(SpriteBatch spriteBatch)
    {
        int t = 1;   // border thickness
        for (int i = 0; i < cols * rows; i++)
        {
            Vector2 pos = GetSlotPosition(i);
            int x = (int)pos.X, y = (int)pos.Y;

            // top, bottom, left, right edges
            spriteBatch.Draw(Button.Pixel, new Rectangle(x, y, cellW, t), Color.Red);
            spriteBatch.Draw(Button.Pixel, new Rectangle(x, y + cellH - t, cellW, t), Color.Red);
            spriteBatch.Draw(Button.Pixel, new Rectangle(x, y, t, cellH), Color.Red);
            spriteBatch.Draw(Button.Pixel, new Rectangle(x + cellW - t, y, t, cellH), Color.Red);
        }
    }
    
    
    public void UpdatePanel(GameTime gameTime)
    {
        foreach (Button button in buttons)
            button.Update(gameTime);
    }

    public void DrawPanel(SpriteBatch spriteBatch)
    {
        panelSprite.Draw(spriteBatch);
        titleText?.Draw(spriteBatch);
        foreach (var button in buttons)
            button.Draw(spriteBatch);
        DrawGridDebug(spriteBatch);
    }

    private Vector2 GetSlotTopRightCorner(int slotIndex)
    {
        return GetSlotPosition(slotIndex) + new Vector2(cellW, 0);
    }
    private Vector2 GetSlotPosition(int slotIndex)
    {
        int col = slotIndex % cols;
        int row = slotIndex / cols;
        return panelSprite.tm.position + new Vector2(col * cellW, row * cellH);
    }

    public Vector2 GetSlotCenter(int slotIndex)
    {
        return GetSlotPosition(slotIndex) + new Vector2(cellW * 0.5f, cellH * 0.5f); 
    }

    public Vector2 GetSlotUpperCenter(int slotIndex)
    {
        return GetSlotCenter(slotIndex) - new Vector2(0, cellH * 0.5f);
    }
    
    public Vector2 GetSlotBottomCenter(int slotIndex)
    {
        return GetSlotCenter(slotIndex) + new Vector2(0, cellH * 0.5f);
    }

    public void AddButton(int slotIndex, string label, Action onClick, int width = 200, int height = 80)
    {
        buttonSprite = new Sprite("Button1");
        Vector2 btnSize = new Vector2(width /2f, height/2f);//used to center the buttons to the center of the cell
        Vector2 pos = GetSlotCenter(slotIndex) - btnSize;
        Button b = new Button(buttonSprite, pos, width, height);
        b.SetText(label, Game1._font, Color.White);
        b.OnClick += onClick;
        b.Start();
        buttons.Add(b);
    }

    public void AddSpriteButton(int slotIndex, string label, Action onClick, Sprite buttonSprite, int width = 64, int height = 64)
    {
        this.buttonSprite = buttonSprite;
        Vector2 btnSize = new Vector2(width /2f, height/2f);//used to center the buttons to the center of the cell
        Vector2 pos = GetSlotCenter(slotIndex) - btnSize;
        Vector2 textPos = GetSlotUpperCenter(slotIndex);
        Button b = new Button(buttonSprite, pos, width, height);
        b.SetTextAtPos(label,Game1._font, Color.White,textPos);
        b.OnClick += onClick;
        b.Start();
        buttons.Add(b);
    }

    public void AddCloseButton(int slotIndex, Action onClick)
    {
        buttonSprite = new Sprite("CloseButton64");
        float btnWidth = buttonSprite.texture.Width;
        //float btnHeight = buttonSprite.texture.Height;
        Vector2 pos = GetSlotTopRightCorner(slotIndex) - new Vector2(btnWidth, 0);
        Button b = new Button(buttonSprite, pos, 64, 64);
        b.OnClick += onClick;
        b.Start();
        buttons.Add(b);
    }
    
    public void SetTitle(string title)
    {
        titleText = new Text
        {
            text = title,
            font = Game1._font,
            color = Color.White,
            sortingOrder = 1f
        };
        titleText.tm.scale = new Vector2(1.5f, 1.5f);
        titleText.tm.position = panelSprite.tm.position + new Vector2(cellW * cols * 0.5f, titleOffsetY);
    }
    
    
}