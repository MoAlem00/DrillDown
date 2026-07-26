using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class MainMenu : IDrawable,IUpdatable
{
    private Sprite background;
    private Panel menuPanel;

    public MainMenu(Sprite background)
    {
        this.background = background;
        menuPanel = new Panel(new Sprite("Panel1"), 3, 4,1.2f,300);
        menuPanel.SetTitle("Main Menu");
        menuPanel.AddButton(4,"Start",() => GameManager.Instance.StartGame());
        menuPanel.AddButton(7,"Settings",() => Console.WriteLine("Settings"));
        menuPanel.AddButton(10,"Exit",() => GameManager.Instance.QuitGame());
    }
    public void Start()
    {
    }

    public void Update(GameTime gameTime)
    {
        menuPanel.UpdatePanel(gameTime);
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        background.Draw(spriteBatch);
        menuPanel.DrawPanel(spriteBatch);
    }
}