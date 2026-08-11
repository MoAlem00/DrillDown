using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class StartMenu : Menu
{
    private Sprite background;
    private Text introText;

    private string intro =
        "An unstable portal lies buried at the planet's core.\n" +
        "Drill down. Mine to fund your descent.\n" +
        "Reach it and open the door to other worlds.\n\n" +
        "Good luck, Pilot.";
    
    public StartMenu(Sprite background)
    {
        this.background = background;
        panel = new Panel(new Sprite("Panel1"), 3, 6,1.5f,300);
        introText = Text.CreateDefault(intro);
        panel.SetTitle("Drill Down");
        introText.tm.position = panel.GetPanelCenter() + new Vector2(0,-120f);
        panel.AddButton(10,"Start",() => GameManager.Instance.StartGame());
        panel.AddButton(13,"Settings",() => Console.WriteLine("Settings"));
        panel.AddButton(16,"Exit",() => GameManager.Instance.QuitGame());
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        background.Draw(spriteBatch);
        base.Draw(spriteBatch);
        introText.Draw(spriteBatch);
    }
}