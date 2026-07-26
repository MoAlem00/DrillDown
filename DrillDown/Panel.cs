using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
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
    }


    public Vector2 GetSlotPosition(int slotIndex)
    {
        int col = slotIndex % cols;
        int row = slotIndex / cols;
        return panelSprite.tm.position + new Vector2(col * cellW, row * cellH);
    }

    public Vector2 GetSlotCenter(int slotIndex)
    {
        return GetSlotPosition(slotIndex) + new Vector2(cellW * 0.5f, cellH * 0.5f); 
    }

    public void AddButton(int slotIndex, string label, Action onClick, int width = 200, int height = 80)
    {
        buttonSprite = new Sprite("Button1");
        Vector2 pos = GetSlotPosition(slotIndex) + new Vector2(padding, padding);
        Button b = new Button(buttonSprite, pos, width, height);
        b.SetText(label, Game1._font, Color.White);
        b.OnClick += onClick;
        b.Start();
        buttons.Add(b);
    }

    public void AddCloseButton(int slotIndex, Action onClick)
    {
        buttonSprite = new Sprite("CloseButton64");
        Vector2 pos = GetSlotPosition(slotIndex) + new Vector2(padding * 7, padding*2);
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